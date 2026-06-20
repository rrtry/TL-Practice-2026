using Fighters.Services.Randomization;

namespace Fighters.Models.Fighters;

public interface IFighter : INamed
{
    int GetCurrentHealth();
    int GetMaxHealth();
    int CalculateDamage();
    int CalculateArmor();
    int GetInitiative();
    DamageStats TakeDamage( IRandomService random, IFighter attacker );
}