using CarFactory.Models.Interfaces;

namespace CarFactory.Models.Implementations.Bodies;

public class HatchbackBody : IBody
{
    public string Name => "Hatchback";
    public double AerodynamicFactor => 0.95;
}