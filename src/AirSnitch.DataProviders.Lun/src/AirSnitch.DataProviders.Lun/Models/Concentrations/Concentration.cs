using Index = AirSnitch.DataProviders.Lun.Models.Concentrations.Index;

namespace AirSnitch.DataProviders.Lun.Models.Concentrations;

public class Concentration
{
    public double Min { get; set; }
    public double Max { get; set; }
    public Index Index { get; set; }
}