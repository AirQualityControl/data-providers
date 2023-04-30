using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using AirSnitch.DataProiders.Common;
using AirSnitch.DataProviders.Lun.Models;
using AirSnitch.DataProviders.Lun.OSM;
using AirSnitch.DataProviders.Lun.Services;
using AirSnitch.SDK;
using AirSnitch.SDK.Measurements;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirSnitch.DataProviders.Lun;

public class Function
{
	public async Task FunctionHandler(Dictionary<string, string> input, ILambdaContext context) {
		await SubmitDataPoints();
	}

	private  async Task SubmitDataPoints() {
		var client = new HttpClient();
		var measurements = await GetLunAirStationMeasurements(client);
		await SlowStrategy(measurements, client);
	}

	private async Task SlowStrategy(List<Mesurement> measurements, HttpClient client) {
		var osmService = new OsmGeocodingService(client);
		var sqsConfig = await SqsConfig.CreateAsync();
		foreach (var measurement in measurements) {
			var dataPoint = await GetDataPoint(measurement, osmService);
			if (dataPoint != null) {
				await AirSnitchPlatform.SubmitMeasurement(dataPoint, sqsConfig);
			}
		}
	}
	
	private async Task<IEnumerable<DataPoint>> FastStrategy(IEnumerable<Mesurement> measurements, HttpClient client) {
		var osmService = new OsmGeocodingService(client);
		var result = new ConcurrentBag<DataPoint>();
		var tasks = measurements.Select(async measurement => {
			var dataPoint = await GetDataPoint(measurement, osmService);
			if (dataPoint != null) {
				result.Add(dataPoint);
			}
		});
		await Task.WhenAll(tasks);
		return result;
	}

	private async Task<DataPoint> GetDataPoint(Mesurement measurement, IGeocodingService osmGeocodingService) {
		var address = await osmGeocodingService.GetAddress(
			measurement.Station.Coordinates.Lat, measurement.Station.Coordinates.Lng);

		DataPoint dataPoint = null;

		var particle = measurement.Particles.OrderByDescending(item => item.Time)
			.FirstOrDefault(p => p.Pm2_5.HasValue && p.Pm2_5.Value != 0 && p.Pm10.HasValue && p.Pm10.Value != 0);
		if (particle != null) {
			var calculatedPollutionValue = AqiCalculator.CalculateAqi(particle);

			var weather = measurement.Weather.OrderByDescending(item => item.Time)
				.FirstOrDefault(measure =>
					measure.Humidity.HasValue && measure.Pressure.HasValue && measure.Temperature.HasValue);

			dataPoint = GetDataPoint(measurement, address, particle, weather, calculatedPollutionValue);
		}

		return dataPoint;
	}

	private DataPoint GetDataPoint(AirSnitch.DataProviders.Lun.Models.Mesurement measurement,
			Address address, Particle particle, Weather weather, int aqi) {
		var dataPoint = new DataPoint() {
			StationInfo = new StationInfo() {
				StationId = measurement.Station.Name,
				CityName = address.City,
				CountryCode = address.CountryCode.ToUpper(),
				Address = address.Road + ", " + address.HouseNomber,
				GeoCoordinates = new GeoCoordinates() {
					Latitude = measurement.Station.Coordinates.Lat,
					Longitude = measurement.Station.Coordinates.Lng
				},
				StationName = "Lun Station #" + measurement.Station.Name
			},
			DataProviderInfo = new() {
				Name = Constants.ProviderName,
				Uri = Constants.ProviderUri,
				Tag = Constants.ProviderId
			},
			Measurements = GetDataPointMesurements(particle, weather),
			IndexValue = new() { IndexValue = aqi, IndexName = "US_AIQ" },
			DateTime = particle.Time,
		};
		return dataPoint;
	}

	private List<SDK.Measurements.Measurement> GetDataPointMesurements(Particle particle, Weather weather) {
		var result = new List<SDK.Measurements.Measurement>();

		if (particle.Pm1.HasValue) {
			result.Add(new() { Name = "PM1", Value = particle.Pm1.Value });
		}
		if (particle.Pm2_5.HasValue) {
			result.Add(new() { Name = "PM25", Value = particle.Pm2_5.Value });
		}
		if (particle.Pm10.HasValue) {
			result.Add(new() { Name = "PM10", Value = particle.Pm10.Value });
		}
		if (weather != null) {
			if (weather.Humidity.HasValue) {
				result.Add(new() { Name = "Humidity", Value = weather.Humidity });
			}
			if (weather.Temperature.HasValue) {
				result.Add(new() { Name = "Temperature", Value = weather.Temperature });
			}
			if (weather.Pressure.HasValue) {
				result.Add(new() { Name = "Pressure", Value = weather.Pressure });
			}
		}
		
		return result;
	}
	
	private async Task<List<Mesurement>> GetLunAirStationMeasurements(HttpClient client) {
		var dataService = new LunDataService(client);
		var measurements = await dataService.GetMeasurements();
		return measurements;
	}
}