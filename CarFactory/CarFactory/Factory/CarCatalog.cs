using CarFactory.Models.Bodies;
using CarFactory.Models.Brands;
using CarFactory.Models.Colors;
using CarFactory.Models.Engines;
using CarFactory.Models.Transmissions;
using CarFactory.Models.Wheels;

namespace CarFactory.Factory;

public static class CarCatalog
{
    private static readonly ToyotaBrand _toyotaBrand = new();
    private static readonly BMWBrand _bmwBrand = new();
    private static readonly SubaruBrand _subaruBrand = new();

    private static readonly RedColor _redColor = new();
    private static readonly BlueColor _blueColor = new();

    private static readonly SedanBody _sedanBody = new();
    private static readonly HatchbackBody _hatchbackBody = new();
    private static readonly SUVBody _suvBody = new();

    private static readonly PetrolEngine _petrolEngine = new();
    private static readonly DiselEngine _diselEngine = new();

    private static readonly LightWheels _lightWheels = new();
    private static readonly HeavyWheels _heavyWheels = new();

    private static readonly AutomaticTransmission _automaticTransmission = new();
    private static readonly ManualTransmission _manualTransmission = new();

    public static readonly IReadOnlyList<IBrand> Brands = new List<IBrand>
    {
        _toyotaBrand,
        _bmwBrand,
        _subaruBrand
    };

    public static readonly IReadOnlyList<IColor> Colors = new List<IColor>
    {
        _redColor,
        _blueColor
    };

    public static readonly IReadOnlyList<IBody> Bodies = new List<IBody>
    {
        _sedanBody,
        _hatchbackBody,
        _suvBody
    };

    public static readonly IReadOnlyList<IEngine> Engines = new List<IEngine>
    {
        _petrolEngine,
        _diselEngine
    };

    public static readonly IReadOnlyList<IWheels> Wheels = new List<IWheels>
    {
        _lightWheels,
        _heavyWheels
    };

    public static readonly IReadOnlyList<ITransmission> Transmissions = new List<ITransmission>
    {
        _automaticTransmission,
        _manualTransmission
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
            () =>
            {
                return new CarBuilder()
                .SetBrand( _toyotaBrand )
                .SetColor( _blueColor )
                .SetBody( _sedanBody )
                .SetEngine( _petrolEngine )
                .SetTransmission( _automaticTransmission )
                .SetWheels( _lightWheels );
            }
        );

    public static CarPreset ConfigureSportsHatch() =>
        new CarPreset(
            "Sports Hatch",
            () =>
            {
                return new CarBuilder()
                .SetBrand( _bmwBrand )
                .SetColor( _redColor )
                .SetBody( _hatchbackBody )
                .SetEngine( _petrolEngine )
                .SetTransmission( _manualTransmission )
                .SetWheels( _lightWheels );
            }
        );

    public static CarPreset ConfigureSUVOffroad() =>
        new CarPreset(
            "SUV Offroad",
            () =>
            {
                return new CarBuilder()
                .SetBrand( _toyotaBrand )
                .SetColor( _blueColor )
                .SetBody( _suvBody )
                .SetEngine( _diselEngine )
                .SetTransmission( _automaticTransmission )
                .SetWheels( _heavyWheels );
            }
        );

    public static CarPreset ConfigureFamilyHatchback() =>
        new CarPreset(
            "Family Hatchback",
            () =>
            {
                return new CarBuilder()
                .SetBrand( _subaruBrand )
                .SetColor( _blueColor )
                .SetBody( _hatchbackBody )
                .SetEngine( _petrolEngine )
                .SetTransmission( _manualTransmission )
                .SetWheels( _lightWheels );
            }
        );
}