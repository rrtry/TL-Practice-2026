namespace CarFactory.Factory;

public class CarPreset
{
    public string Name { get; }

    public CarBuilder Configuration { get; }

    public CarPreset( string name, CarBuilder configuration )
    {
        Name = name;
        Configuration = configuration;
    }
}