using AirSnitch.DataProiders.Common;
using AirSnitch.DataProviders.EcoCity.Models;
using AirSnitch.DataProviders.EcoCity.Services;
using AirSnitch.SDK;
using AirSnitch.SDK.Measurements;
using Amazon.Lambda.Core;
using System.Diagnostics.Metrics;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirSnitch.DataProviders.EcoCity;

public class Function
{
	
	public async Task FunctionHandler(Dictionary<string,string> input, ILambdaContext context)
	{
		await SubmitDataPoints();
	}

	private async Task SubmitDataPoints() {
		var apiKey = await EcoCityRequestUtilities.GetApiKeyAsync();
		var actualStations = await EcoCityDataService.GetLatestDataAsync(apiKey);
		var sqsConfig = await SqsConfig.CreateAsync();
		var tasks = actualStations.Select(async measurement => {
			var dataPoint = await GetDataPoint(measurement, apiKey);
			if (dataPoint != null) {
				await AirSnitchPlatform.SubmitMeasurement(dataPoint, sqsConfig);
			}
		});
		await Task.WhenAll(tasks);
	}

	private async Task<DataPoint> GetDataPoint(Models.StationInfo stationInfo, string apiKey) {
		var response = new EcoCityResponse(await EcoCityDataService.GetStationMeasurements(apiKey, stationInfo.Id));
		var dataPoint = new DataPoint() {
			StationInfo = new SDK.StationInfo() {
				StationId = stationInfo.Id,
				CityName = stationInfo.CityName,
				CountryCode = "UA",
				Address = stationInfo.LocalName,
				GeoCoordinates = new GeoCoordinates() {
					Latitude = Double.Parse(stationInfo.Latitude),
					Longitude = Double.Parse(stationInfo.Longitude)
				},
				StationName = "EcoCity Station #" + stationInfo.Id + " " + stationInfo.StationName
			},
			DataProviderInfo = new () {
				Name = Constants.ProviderName,
				Uri = Constants.ProviderUri,
				Tag = Constants.ProviderId
			},
			Measurements = GetDataPointMesurements(response),
			IndexValue = new() { IndexValue =response.Index, IndexName = "US_AIQ" },
			DateTime = response.MeasurementDateTime,
		};
		return dataPoint;
	}

	private List<Measurement> GetDataPointMesurements(EcoCityResponse response) {
		var result = new List<SDK.Measurements.Measurement>();

		if (response.PM1.HasValue) {
			result.Add(new() { Name = "PM1", Value = response.PM1.Value });
		}
		if (response.PM2_5.HasValue) {
			result.Add(new() { Name = "PM25", Value = response.PM2_5.Value });
		}
		if (response.PM10.HasValue) {
			result.Add(new() { Name = "PM10", Value = response.PM10.Value });
		}
		if (response.Humidity.HasValue) {
			result.Add(new() { Name = "Humidity", Value = response.Humidity });
		}
		if (response.Temperature.HasValue) {
			result.Add(new() { Name = "Temperature", Value = response.Temperature });
		}
		if (response.Pressure.HasValue) {
			result.Add(new() { Name = "Pressure", Value = response.Pressure });
		}
		
		return result;
		
	}
}
