using AirSnitch.DataProviders.EcoCity.DictionaryToListJsonConverter;
using Newtonsoft.Json;

namespace AirSnitch.DataProviders.EcoCity.Models;

public class Weather
{
	[JsonProperty("temp")]
	public string Temperature { get; set; }
	[JsonProperty("pressure")]
	public string Pressure { get; set; }
	[JsonProperty("humidity")]
	public string Humidity { get; set; }
	[JsonProperty("speed")]
	public string WindSpeed { get; set; }
	[JsonProperty("deg")]
	public string Deg { get; set; }
	[JsonProperty("unit")]
	public string Unit { get; set; }
	[JsonProperty("time")]
	public string Time { get; set; }
	[JsonProperty("localName")]
	public string LocalName { get; set; }
	[JsonProperty("id")]
	public string Id { get; set; }
}

public class DynamicO2
{
	[JsonProperty("value")]
	public int Value { get; set; }
	[JsonProperty("source")]
	public List<double> Source { get; set; }
}

public class Owner
{
	[JsonProperty("provider")]
	public string Provider { get; set; }
}

public class Notify
{
	public string State { get; set; }
	public string Resource { get; set; }
}

public class AirPollutionResultDto
{
	[JsonProperty("id")]
	public string Id { get; set; }
	[JsonProperty("name")]
	public string Name { get; set; }
	[JsonProperty("unit")]
	public string Unit { get; set; }
	[JsonProperty("cr")]
	public string Cr { get; set; }
	[JsonProperty("value")]
	public string Value { get; set; }
	[JsonProperty("localName")]
	public string LocalName { get; set; }
	[JsonProperty("localUnit")]
	public string LocalUnit { get; set; }
	[JsonProperty("timeMin")]
	public string TimeMin { get; set; }
	[JsonProperty("timeMax")]
	public string TimeMax { get; set; }
	[JsonProperty("share")]
	public string Share { get; set; }
	[JsonProperty("levels")]
	public string Levels { get; set; }
	[JsonProperty("offset")]
	public string Offset { get; set; }
	[JsonProperty("level")]
	public int Level { get; set; }
	[JsonProperty("index")]
	public int? Index { get; set; }
	[JsonProperty("weather")]
	public Weather Weather { get; set; }
	[JsonProperty("dynamicO2")]
	public DynamicO2 DynamicO2 { get; set; }
	[JsonProperty("owner")]
	public Owner Owner { get; set; }

	public Notify Notify { get; set; }

}

public class AirPollutionResponse
{
	[JsonConverter(typeof(DictionaryToListConverter))]
	public List<AirPollutionResultDto> AirPollutionResultDtos { get; set; }
}