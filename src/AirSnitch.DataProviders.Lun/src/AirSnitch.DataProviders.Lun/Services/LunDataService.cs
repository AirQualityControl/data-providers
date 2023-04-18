using AirSnitch.DataProviders.Lun.Models;
using Newtonsoft.Json;

namespace AirSnitch.DataProviders.Lun.Services;

public class LunDataService
{
	private readonly HttpClient _client;

	private const string Uri = "https://misto.lun.ua/api/air/v1/public/";

	public LunDataService(HttpClient httpClient)
	{
		_client = httpClient;
	}

	public async Task<List<Mesurement>> GetMeasurements()
	{
		var response = await _client.GetAsync(Uri + "data");
		var resp = await response.Content.ReadAsStringAsync();
		var measurements = JsonConvert.DeserializeObject<List<Mesurement>>(resp);
		return measurements;
	}
		
}