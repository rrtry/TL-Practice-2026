namespace CarFactory.Models.Interfaces;

public interface IEngine : INamed
{
    int Horsepower { get; }
    double Displacement { get; }
}