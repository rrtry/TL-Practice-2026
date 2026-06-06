namespace Fighters.Services.Randomization;

public interface IRandomService
{
    int Next( int minValue, int maxValue );
    int Next( int maxValue );
    double NextDouble();
}
