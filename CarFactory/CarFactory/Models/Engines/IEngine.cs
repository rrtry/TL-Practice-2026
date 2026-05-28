namespace CarFactory.Models.Engines;

public interface IEngine : INamed
{
    int Horsepower { get; }
    double Displacement { get; }
}