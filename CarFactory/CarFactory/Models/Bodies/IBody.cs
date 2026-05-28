namespace CarFactory.Models.Bodies;

public interface IBody : INamed
{
    double AerodynamicFactor { get; }
}