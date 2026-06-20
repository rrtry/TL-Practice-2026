namespace CarFactory.Models.Wheels;

public interface IWheels : INamed
{
    double WeightFactor { get; }
}