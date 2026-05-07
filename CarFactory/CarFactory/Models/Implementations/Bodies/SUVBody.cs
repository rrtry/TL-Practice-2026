using CarFactory.Models.Interfaces;

namespace CarFactory.Models.Implementations.Bodies;

public class SUVBody : IBody
{
    public string Name => "SUV";
    public double AerodynamicFactor => 0.9;
}