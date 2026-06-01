using CarFactory.Models;
using CarFactory.Models.Bodies;
using CarFactory.Models.Brands;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;
using CarFactory.Models.Wheels;

namespace CarFactory.Factory;

/// <summary>
/// Builder для создания экземпляров авто. Используется единожды для создания каждого экземпляра.
/// </summary>
public class CarBuilder
{
    private IBrand? _brand;
    private IColor? _color;
    private IBody? _body;
    private IEngine? _engine;
    private ITransmission? _transmission;
    private IWheels? _wheels;

    public CarBuilder()
    {
    }

    public CarBuilder SetBrand( IBrand brand )
    {
        _brand = brand;
        return this;
    }

    public CarBuilder SetColor( IColor color )
    {
        _color = color;
        return this;
    }

    public CarBuilder SetBody( IBody body )
    {
        _body = body;
        return this;
    }

    public CarBuilder SetEngine( IEngine engine )
    {
        _engine = engine;
        return this;
    }

    public CarBuilder SetTransmission( ITransmission transmission )
    {
        _transmission = transmission;
        return this;
    }

    public CarBuilder SetWheels( IWheels wheels )
    {
        _wheels = wheels;
        return this;
    }

    public Car Build()
    {
        if ( _brand == null || _color == null || _body == null ||
            _engine == null || _transmission == null || _wheels == null )
        {
            throw new InvalidOperationException( "Cannot build a car: not all components are set. " );
        }

        return new Car( _brand, _color, _body, _engine, _transmission, _wheels );
    }
}