using CarFactory.Models.Interfaces;

namespace CarFactory.Models.Implementations.Wheels;

public class LightWheels : IWheels
{
    public string Name => "Light Wheels";
    public double WeightFactor => 1.0;
}