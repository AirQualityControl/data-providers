namespace AirSnitch.DataProviders.Lun.Models;

public class Mesurement
{
	public Station Station { get; set; }
	public List<Particle> Particles { get; set; }
	public List<Weather> Weather { get; set; }
}