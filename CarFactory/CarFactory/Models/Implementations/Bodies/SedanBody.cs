using CarFactory.Models.Interfaces;

namespace CarFactory.Models.Implementations.Bodies;

public class SedanBody : IBody
{
    public string Name => "Sedan";
    public double AerodynamicFactor => 1.0;
}