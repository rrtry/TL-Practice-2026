using CarFactory.Factory;
using CarFactory.Models.Implementations.Bodies;
using CarFactory.Models.Implementations.Brands;
using CarFactory.Models.Implementations.Colors;
using CarFactory.Models.Implementations.Engines;
using CarFactory.Models.Implementations.Transmissions;
using CarFactory.Models.Implementations.Wheels;
using CarFactory.Models.Interfaces;

namespace CarFactory.Models;

public static class CarCatalog
{
    public static IReadOnlyList<CarPreset> Presets = new List<CarPreset>
    {
        CarConfiguration.ConfigureCitySedan(),
        CarConfiguration.ConfigureSportsHatch(),
        CarConfiguration.ConfigureSUVOffroad(),
        CarConfiguration.ConfigureFamilyHatchback()
    };

    public static IReadOnlyList<IBrand> Brands { get; } = new List<IBrand>
    {
        new ToyotaBrand(),
        new BMWBrand(),
        new SubaruBrand()
    };

    public static IReadOnlyList<IColor> Colors { get; } = new List<IColor>
    {
        new RedColor(),
        new BlueColor()
    };

    public static IReadOnlyList<IBody> Bodies { get; } = new List<IBody>
    {
        new SedanBody(),
        new HatchbackBody(),
        new SUVBody()
    };

    public static IReadOnlyList<IEngine> Engines { get; } = new List<IEngine>
    {
        new PetrolEngine(),
        new DieselEngine()
    };

    public static IReadOnlyList<IWheels> Wheels { get; } = new List<IWheels>
    {
        new LightWheels(),
        new HeavyWheels()
    };

    public static IReadOnlyList<ITransmission> Transmissions { get; } = new List<ITransmission>
    {
        new AutomaticTransmission(),
        new ManualTransmission()
    };
}