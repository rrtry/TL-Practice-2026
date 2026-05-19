namespace CarFactory.Models.Interfaces;

public interface ITransmission : INamed
{
    int GearCount { get; }
    double EfficiencyFactor { get; }
}