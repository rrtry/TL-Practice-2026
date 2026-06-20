namespace Fighters.Services.Randomization;

public interface IRandomService
{
    int Next( int maxValue );
    double NextDouble();
}
