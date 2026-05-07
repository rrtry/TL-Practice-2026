using CarFactory.Models.Interfaces;

namespace CarFactory.Models;

public class Car
{
    public IBrand Brand { get; }
    public IColor Color { get; }
    public IBody Body { get; }
    public IEngine Engine { get; }
    public ITransmission Transmission { get; }
    public IWheels Wheels { get; }
    public int MaxSpeed { get; }

    public Car(
        IBrand brand,
        IColor color,
        IBody body,
        IEngine engine,
        ITransmission transmission,
        IWheels wheels )
    {
        Brand = brand;
        Color = color;
        Body = body;
        Engine = engine;
        Transmission = transmission;
        Wheels = wheels;
        MaxSpeed = CalculateMaxSpeed();
    }

    private int CalculateMaxSpeed()
    {
        const double horsePowerFactor = 1.2;
        const double minGearCount = 5;
        const double maxGearBonus = 0.1;
        const double gearBonusFactor = 0.01;

        double speed = Engine.Horsepower * horsePowerFactor;

        speed *= Body.AerodynamicFactor;
        speed *= Transmission.EfficiencyFactor;
        speed *= Wheels.WeightFactor;

        double gearBonus = 1 + Math.Min( maxGearBonus, ( Transmission.GearCount - minGearCount ) * gearBonusFactor );
        speed *= gearBonus;

        return ( int )Math.Round( speed );
    }

    public override string ToString()
    {
        return $"\nCar: \n" +
               $"Brand:           {Brand.Name}\n" +
               $"Color:           {Color.Name}\n" +
               $"Body type:       {Body.Name}\n" +
               $"Engine:          {Engine.Name} ({Engine.Horsepower} HP, {Engine.Displacement}L)\n" +
               $"Transmission:    {Transmission.Name} ({Transmission.GearCount} gears)\n" +
               $"Wheels:          {Wheels.Name}\n" +
               $"Max speed:       {MaxSpeed} km/h\n";
    }
}