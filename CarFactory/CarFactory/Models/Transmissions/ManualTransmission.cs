namespace CarFactory.Models.Transmissions;

public class ManualTransmission : ITransmission
{
    public string Name => "Manual";
    public int GearCount => 6;
    public double EfficiencyFactor => 1.1;
}