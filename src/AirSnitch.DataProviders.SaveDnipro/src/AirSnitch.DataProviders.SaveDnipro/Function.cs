using AirSnitch.DataProiders.Common;
using AirSnitch.DataProviders.SaveDnipro.Models;
using AirSnitch.DataProviders.SaveDnipro.Services;
using AirSnitch.SDK;
using AirSnitch.SDK.Measurements;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AirSnitch.DataProviders.SaveDnipro;

public class Function
{
	/// <summary>
	/// A simple function that takes a string and does a ToUpper
	/// </summary>
	/// <param name="input"></param>
	/// <param name="context"></param>
	/// <returns></returns>
	public async Task FunctionHandler(Dictionary<string, string> input, ILambdaContext context)
	{
		var saveDniproPlatform = new SaveDniproPlatform();
		var dataStream = saveDniproPlatform.GetSensorsDataStream();
		var sqsConfig = await SqsConfig.CreateAsync();
		foreach (var saveDniproSensorData in dataStream)
		{
			if (IsSensorDataActual(saveDniproSensorData))
			{
				var dataPoint = GenerateDataPoint(saveDniproSensorData);
				await AirSnitchPlatform.SubmitMeasurement(dataPoint, sqsConfig);
			}
			else
			{
				context.Logger.LogInformation(
					$"Data for station {saveDniproSensorData.Id} is not actual. Last measurement was at {saveDniproSensorData.GetLastMeasurementUtcDate()}"
				);
			}
		}
	}
		
	private static DataPoint GenerateDataPoint(SaveDniproSensorData sensorData)
	{
		var dataPoint = new DataPoint()
		{
			StationInfo = new StationInfo()
			{
				StationId = sensorData.Id,
				CityName = sensorData.CityName,
				CountryCode = "UA",
				Address = sensorData.StationName,
				GeoCoordinates = new GeoCoordinates()
				{
					Latitude = Double.Parse(sensorData.Latitude),
					Longitude = Double.Parse(sensorData.Longitude)
				},
				StationName = sensorData.Id
			},
			DataProviderInfo = new() {
				Name = Constants.ProviderName,
				Uri = Constants.ProviderUri,
				Tag = Constants.ProviderId
			},
			Measurements = new List<Measurement>()
			{
				sensorData.GetPm10(),
				sensorData.GetPM25(),
				sensorData.GetHumidity(),
				sensorData.GetTemperature(),
			},
			IndexValue = sensorData.GetUsAirQualityIndex(),
			DateTime = sensorData.GetLastMeasurementUtcDate(),
		};
		return dataPoint;
	}

	private static bool IsSensorDataActual(SaveDniproSensorData sensorData)
	{
		var currentTimeInUtc = DateTime.UtcNow;
		return currentTimeInUtc - sensorData.GetLastMeasurementUtcDate() <= TimeSpan.FromHours(2);
	}
}