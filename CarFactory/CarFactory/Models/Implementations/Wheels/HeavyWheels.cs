using CarFactory.Models.Interfaces;

namespace CarFactory.Models.Implementations.Wheels;

public class HeavyWheels : IWheels
{
    public string Name => "Heavy Wheels";
    public double WeightFactor => 0.95;
}