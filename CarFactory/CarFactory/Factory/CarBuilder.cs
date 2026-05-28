using CarFactory.Models;
using CarFactory.Models.Bodies;
using CarFactory.Models.Brands;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;
using CarFactory.Models.Wheels;

namespace CarFactory.Factory;

public class CarBuilder
{
    private IBrand _brand;
    private IColor _color;
    private IBody _body;
    private IEngine _engine;
    private ITransmission _transmission;
    private IWheels _wheels;

    public CarBuilder()
    {
        _brand = new ToyotaBrand();
        _color = new BlueColor();
        _body = new SedanBody();
        _engine = new PetrolEngine();
        _transmission = new AutomaticTransmission();
        _wheels = new LightWheels();
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
        return new Car( _brand, _color, _body, _engine, _transmission, _wheels );
    }
}