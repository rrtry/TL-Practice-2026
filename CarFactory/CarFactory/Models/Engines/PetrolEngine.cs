namespace CarFactory.Models.Engines;

public class PetrolEngine : IEngine
{
    public string Name => "Petrol 2.0";
    public int Horsepower => 180;
    public double Displacement => 2.0;
}