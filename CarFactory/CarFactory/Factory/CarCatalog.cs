using CarFactory.Models.Bodies;
using CarFactory.Models.Brands;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;
using CarFactory.Models.Wheels;

namespace CarFactory.Factory;

public static class CarCatalog
{
    public static readonly IReadOnlyList<IBrand> Brands = new List<IBrand>
    {
        new ToyotaBrand(),
        new BMWBrand(),
        new SubaruBrand()
    };

    public static readonly IReadOnlyList<IColor> Colors = new List<IColor>
    {
        new RedColor(),
        new BlueColor()
    };

    public static readonly IReadOnlyList<IBody> Bodies = new List<IBody>
    {
        new SedanBody(),
        new HatchbackBody(),
        new SUVBody()
    };

    public static readonly IReadOnlyList<IEngine> Engines = new List<IEngine>
    {
        new PetrolEngine(),
        new DieselEngine()
    };

    public static readonly IReadOnlyList<IWheels> Wheels = new List<IWheels>
    {
        new LightWheels(),
        new HeavyWheels()
    };

    public static readonly IReadOnlyList<ITransmission> Transmissions = new List<ITransmission>
    {
        new AutomaticTransmission(),
        new ManualTransmission()
    };

    public static readonly IReadOnlyList<CarPreset> Presets = new List<CarPreset>
    {
        ConfigureCitySedan(),
        ConfigureSportsHatch(),
        ConfigureSUVOffroad(),
        ConfigureFamilyHatchback()
    };

    public static CarPreset ConfigureCitySedan() =>
        new CarPreset(
            "City Sedan",
            new CarBuilder()
                .SetBrand( new ToyotaBrand() )
                .SetColor( new BlueColor() )
                .SetBody( new SedanBody() )
                .SetEngine( new PetrolEngine() )
                .SetTransmission( new AutomaticTransmission() )
                .SetWheels( new LightWheels() )
        );

    public static CarPreset ConfigureSportsHatch() =>
        new CarPreset(
            "Sports Hatch",
            new CarBuilder()
                .SetBrand( new BMWBrand() )
                .SetColor( new RedColor() )
                .SetBody( new HatchbackBody() )
                .SetEngine( new PetrolEngine() )
                .SetTransmission( new ManualTransmission() )
                .SetWheels( new LightWheels() )
        );

    public static CarPreset ConfigureSUVOffroad() =>
        new CarPreset(
            "SUV Offroad",
            new CarBuilder()
                .SetBrand( new ToyotaBrand() )
                .SetColor( new BlueColor() )
                .SetBody( new SUVBody() )
                .SetEngine( new DieselEngine() )
                .SetTransmission( new AutomaticTransmission() )
                .SetWheels( new HeavyWheels() )
        );

    public static CarPreset ConfigureFamilyHatchback() =>
        new CarPreset(
            "Family Hatchback",
            new CarBuilder()
                .SetBrand( new SubaruBrand() )
                .SetColor( new BlueColor() )
                .SetBody( new HatchbackBody() )
                .SetEngine( new PetrolEngine() )
                .SetTransmission( new ManualTransmission() )
                .SetWheels( new LightWheels() )
        );
}