using System.Collections.Generic;
using System.Net;
using AirSnitch.DataProviders.EcoCity.DictionaryToListJsonConverter;
using AirSnitch.DataProviders.EcoCity.Models;
using AirSnitch.SDK;
using Newtonsoft.Json;

namespace AirSnitch.DataProviders.EcoCity.Services;

public static class EcoCityDataService
{

	public static async Task<List<Models.StationInfo>> GetLatestDataAsync(string apiKey) {
		HttpClient client = new HttpClient();
		var response = await client.GetAsync($"https://eco-city.org.ua/public.json?key={apiKey}&all");
		var resp = await response.Content.ReadAsStringAsync();
		var stations = JsonConvert.DeserializeObject<List<Models.StationInfo>>(resp);
		return stations;
	}


	public static async Task<List<AirPollutionResultDto>> GetStationMeasurements(string apiKey, string stationId) {
		HttpClient client = new HttpClient();
		var response = await client.GetAsync($"https://eco-city.org.ua/public.json?key={apiKey}&id={stationId}&timeShift=0");
		var resp = await response.Content.ReadAsStringAsync();
		var stationMeasurement = JsonConvert.DeserializeObject<List<AirPollutionResultDto>>(resp, new DictionaryToListConverter());
		return stationMeasurement;
	}
}