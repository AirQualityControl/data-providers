using AirSnitch.DataProviders.Lun.Models;
using AirSnitch.DataProviders.Lun.Models.Concentrations;
using Index = AirSnitch.DataProviders.Lun.Models.Concentrations.Index;

namespace AirSnitch.DataProviders.Lun.Services;

public class AqiCalculator
{
	private static readonly ConcentrationSummary ConcentrationSummary = new ConcentrationSummary
	{
		Code = "PM2.5",
		Unit = "ug/m3",
		Period = "24h",
		Concentrations = new List<Concentration>
		{
			new() { Min = 0, Max = 12, Index = new Index { Min = 0, Max = 50} },
			new() { Min = 12.1, Max = 35.4, Index = new Index { Min = 51, Max = 100} },
			new() { Min = 35.5, Max = 55.4, Index = new Index { Min = 101, Max = 150} },
			new() { Min = 55.5, Max = 150.4, Index = new Index { Min = 151, Max = 200} },
			new() { Min = 150.5, Max = 250.4, Index = new Index { Min = 201, Max = 300} },
			new() { Min = 250.5, Max = 350.4, Index = new Index { Min = 301, Max = 400} },
			new() { Min = 350.5, Max = 500.4, Index = new Index { Min = 401, Max = 500} },
			new() { Min = 500.5, Max = 5004, Index = new Index { Min = 501, Max = 5e3} }
		}
	};

	public static int CalculateAqi(Particle particle)
	{
		int nowcastConcentrations = (int)(Math.Round(10 * particle.Pm2_5.Value) / 10);
		var concentrationRange = ConcentrationSummary.Concentrations.First(item => nowcastConcentrations >= item.Min && nowcastConcentrations <= item.Max);

		var aqi = (concentrationRange.Index.Max - concentrationRange.Index.Min)
			/ (concentrationRange.Max - concentrationRange.Min)
			* (nowcastConcentrations - concentrationRange.Min) + concentrationRange.Index.Min;

		return (int)Math.Round(aqi);
	}

}