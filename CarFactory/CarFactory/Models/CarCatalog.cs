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
    public static readonly IReadOnlyList<CarPreset> Presets = new List<CarPreset>
    {
        CarConfiguration.ConfigureCitySedan(),
        CarConfiguration.ConfigureSportsHatch(),
        CarConfiguration.ConfigureSUVOffroad(),
        CarConfiguration.ConfigureFamilyHatchback()
    };

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
}