namespace CarFactory.Models.Engines;

public class DieselEngine : IEngine
{
    public string Name => "Diesel 2.0";
    public int Horsepower => 150;
    public double Displacement => 2.0;
}