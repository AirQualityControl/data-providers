using AirSnitch.DataProviders.Lun.OSM;

namespace AirSnitch.DataProviders.Lun.Services;

public interface IGeocodingService
{
	Task<Address> GetAddress(double lat, double lng);
	Task<(double lat, double lng)> GetLatLng(string houseNumber, string street, string city, string country);
}