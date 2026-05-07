namespace CarFactory.Models.Interfaces;

public interface ITransmission : IDisplay
{
    int GearCount { get; }
    double EfficiencyFactor { get; }
}