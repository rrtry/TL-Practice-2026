using CarFactory.Models.Bodies;
using CarFactory.Models.Brands;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;
using CarFactory.Models.Wheels;

namespace CarFactory.Models;

public class Car
{
    private readonly IBrand _brand;
    private readonly IColor _color;
    private readonly IBody _body;
    private readonly IEngine _engine;
    private readonly ITransmission _transmission;
    private readonly IWheels _wheels;
    private readonly int _maxSpeed;

    public Car(
        IBrand brand,
        IColor color,
        IBody body,
        IEngine engine,
        ITransmission transmission,
        IWheels wheels )
    {
        _brand = brand;
        _color = color;
        _body = body;
        _engine = engine;
        _transmission = transmission;
        _wheels = wheels;
        _maxSpeed = CalculateMaxSpeed();
    }

    public override string ToString()
    {
        return $"\nCar: \n" +
               $"Brand:           {_brand.Name}\n" +
               $"Color:           {_color.Name}\n" +
               $"Body type:       {_body.Name}\n" +
               $"Engine:          {_engine.Name} ({_engine.Horsepower} HP, {_engine.Displacement}L)\n" +
               $"Transmission:    {_transmission.Name} ({_transmission.GearCount} gears)\n" +
               $"Wheels:          {_wheels.Name}\n" +
               $"Max speed:       {_maxSpeed} km/h\n";
    }

    private int CalculateMaxSpeed()
    {
        const double horsePowerFactor = 1.2;
        const double minGearCount = 5;
        const double maxGearBonus = 0.1;
        const double gearBonusFactor = 0.01;

        double speed = _engine.Horsepower * horsePowerFactor;

        speed *= _body.AerodynamicFactor;
        speed *= _transmission.EfficiencyFactor;
        speed *= _wheels.WeightFactor;

        double gearBonus = 1 + Math.Min( maxGearBonus, ( _transmission.GearCount - minGearCount ) * gearBonusFactor );
        speed *= gearBonus;

        return ( int )Math.Round( speed );
    }
}