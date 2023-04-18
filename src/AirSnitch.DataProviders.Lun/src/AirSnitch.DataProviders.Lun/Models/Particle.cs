using Newtonsoft.Json;

namespace AirSnitch.DataProviders.Lun.Models;

public class Particle
{
	[JsonProperty("time")]
	public DateTime Time { get; set; }

	[JsonProperty("pm10")]
	public double? Pm1 { get; set; }

	[JsonProperty("pm25")]
	public double? Pm2_5 { get; set; }

	[JsonProperty("pm100")]
	public double? Pm10 { get; set; }
}