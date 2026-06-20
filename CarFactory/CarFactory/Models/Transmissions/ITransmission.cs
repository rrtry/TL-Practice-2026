namespace CarFactory.Models.Transmissions;

public interface ITransmission : INamed
{
    int GearCount { get; }
    double EfficiencyFactor { get; }
}