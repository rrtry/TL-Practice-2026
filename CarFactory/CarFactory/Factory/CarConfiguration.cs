using CarFactory.Models.Implementations.Brands;
using CarFactory.Models.Implementations.Colors;
using CarFactory.Models.Implementations.Bodies;
using CarFactory.Models.Implementations.Engines;
using CarFactory.Models.Implementations.Transmissions;
using CarFactory.Models.Implementations.Wheels;

namespace CarFactory.Factory;

public static class CarConfiguration
{
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