namespace AirSnitch.DataProviders.Lun.Models.Concentrations;

public class ConcentrationSummary
{
    public string Code { get; set; }
    public string Unit { get; set; }
    public string Period { get; set; }
    public List<Concentration> Concentrations { get; set; }
}