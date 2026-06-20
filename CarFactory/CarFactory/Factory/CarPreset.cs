namespace CarFactory.Factory;

public class CarPreset
{
    public string Name { get; }
    public Func<CarBuilder> CreateBuilder { get; }

    public CarPreset( string name, Func<CarBuilder> createBuilder )
    {
        Name = name;
        CreateBuilder = createBuilder;
    }
}