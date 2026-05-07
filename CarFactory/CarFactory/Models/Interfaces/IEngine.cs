namespace CarFactory.Models.Interfaces;

public interface IEngine : IDisplay
{
    int Horsepower { get; }
    double Displacement { get; }
}