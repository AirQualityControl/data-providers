using AirSnitch.SDK;
using AirSnitch.SDK.Measurements;
using Newtonsoft.Json;

namespace AirSnitch.DataProviders.SaveDnipro.Services;

public class SaveDniproSensorData
{
	[JsonProperty("Id")]
	public string Id { get; set; }
	[JsonProperty("cityName")]
	public string CityName { get; set; }
	[JsonProperty("localName")]
	public string LocalName { get; set; }
	[JsonProperty("timeZone")]
	public string TimeZone { get; set; }
	[JsonProperty("stationName")]
	public string StationName { get; set; }
	[JsonProperty("street")]
	public string Street { get; set; }
	[JsonProperty("latitude")]
	public string Latitude { get; set; }
	[JsonProperty("longitude")]
	public string Longitude { get; set; }

	[JsonProperty("pollutants")]
	public List<Pollutant> Pollutants { get; set; }

	public override string ToString()
	{
		return $"StationId: {Id} \n" +
				$"StationName: {StationName} \n " +
				$"CityName : {CityName} \n" +
				$"Street : {Street} \n " +
				$"latitude {Latitude} \n" +
				$"longitude {Longitude}";
	}

	public Measurement GetPM25()
	{
		return Pollutants
			.Where(p => p.Name == "PM2.5")
			.Select(p => new Measurement { Name = "PM25", Value = p.Value }).SingleOrDefault();
	}

	public Measurement GetPm10()
	{
		return Pollutants
			.Where(p => p.Name == "PM10")
			.Select(p => new Measurement { Name = "PM10", Value = p.Value }).SingleOrDefault();
	}

	public Measurement GetTemperature()
	{
		return Pollutants
			.Where(p => p.Name == "Temperature")
			.Select(p => new Measurement { Name = "Temperature", Value = p.Value }).SingleOrDefault();
	}

	public Measurement GetHumidity()
	{
		return Pollutants
			.Where(p => p.Name == "Humidity")
			.Select(p => new Measurement { Name = "Humidity", Value = p.Value }).SingleOrDefault();
	}

	public AQIndexValue GetUsAirQualityIndex()
	{
		var airQIndex = Pollutants
			.SingleOrDefault(p => p.Name == "Air Quality Index");

		if (airQIndex != null && airQIndex.Value.HasValue)
		{
			return new AQIndexValue { IndexName = "US_AIQ", IndexValue = (int)airQIndex.Value };
		}

		return default;
	}

	public DateTime GetLastMeasurementUtcDate()
	{
		var lastMeasurementDateTime = Pollutants.Max(
			p => DateTime.TryParse(p.Time, out DateTime time) ? time : DateTime.MinValue
		);
		if (lastMeasurementDateTime != DateTime.MinValue)
		{
			lastMeasurementDateTime -= TimeSpan.FromHours(3);//3hr - shift between provider time zone and UTC
		}
		return lastMeasurementDateTime;
	}

	public class Pollutant
	{
		[JsonProperty("pol")]
		public string Name { get; set; }

		[JsonProperty("unit")]
		public string Unit { get; set; }

		[JsonProperty("time")]
		public string Time { get; set; }

		[JsonProperty("value")]
		public double? Value { get; set; }
	}
}
