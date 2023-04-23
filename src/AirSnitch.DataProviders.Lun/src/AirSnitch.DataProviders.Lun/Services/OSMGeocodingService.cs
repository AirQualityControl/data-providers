using AirSnitch.DataProviders.Lun.OSM;
using Newtonsoft.Json;

namespace AirSnitch.DataProviders.Lun.Services;

public class OsmGeocodingService : IGeocodingService
{
	private readonly HttpClient _client;

	private const string Uri = "http://nominatim.openstreetmap.org";

	public OsmGeocodingService(HttpClient httpClient)
	{
		_client = httpClient;
	}


	public async Task<Address> GetAddress(double lat, double lng)
	{
		try
		{
			_client.DefaultRequestHeaders.Add("User-Agent", "Non commercial usage app");
			var response = await _client.GetAsync(Uri + $"/reverse?format=json&lat={lat}&lon={lng}");
			var resp = await response.Content.ReadAsStringAsync();
			var addressResponse = JsonConvert.DeserializeObject<AddressResponse>(resp);
			return addressResponse.Address;
		}
		catch (Exception e)
		{
			throw new GeocodingException("OSM GetAddress Exception\n" + e.Message, e);
		}
	}
	public async Task<(double lat, double lng)> GetLatLng(string houseNumber, string street, string city, string country)
	{
		try
		{
			_client.DefaultRequestHeaders.Add("User-Agent", "Non comertial usage app");
			var response = await _client.GetAsync(Uri + $"/search?street={houseNumber}, {street}"
			+ $"&city={city}&country={country}&format=json");
			var resp = await response.Content.ReadAsStringAsync();
			var baseGeocodingResponse = JsonConvert.DeserializeObject<List<BaseGeocodingResponse>>(resp);
			if (baseGeocodingResponse.Count == 0)
			{
				throw new GeocodingException("Invalid Address");
			}

			return (double.Parse(baseGeocodingResponse.FirstOrDefault().Lat), double.Parse(baseGeocodingResponse.FirstOrDefault().Lng));
		}
		catch (Exception e)
		{
			throw new GeocodingException("OSM GetLatLng Exception\n" + e.Message, e);
		}
	}
}

public class GeocodingException : Exception
{
	public GeocodingException()
	{ }

	public GeocodingException(string message) : base(message)
	{ }

	public GeocodingException(string message, Exception ex) : base(message, ex)
	{ }
}