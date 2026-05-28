namespace CarFactory.Models.Transmissions;

public class AutomaticTransmission : ITransmission
{
    public string Name => "Automatic";
    public int GearCount => 8;
    public double EfficiencyFactor => 1.0;
}