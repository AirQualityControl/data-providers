namespace AirSnitch.DataProviders.Lun.Models;

public class Weather
{
	public DateTime Time { get; set; }
	public double? Temperature { get; set; }
	public double? Humidity { get; set; }
	public double? Pressure { get; set; }
}