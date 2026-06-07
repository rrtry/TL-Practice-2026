namespace Fighters.Services.Randomization;

public class SystemRandomService : IRandomService
{
    private readonly Random _random;

    public SystemRandomService( Random? random = null )
    {
        _random = random ?? new Random();
    }

    public int Next( int maxValue ) => _random.Next( maxValue );
    public double NextDouble() => _random.NextDouble();
}