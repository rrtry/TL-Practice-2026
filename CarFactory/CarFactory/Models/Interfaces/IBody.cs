namespace CarFactory.Models.Interfaces;

public interface IBody : INamed
{
    double AerodynamicFactor { get; }
}