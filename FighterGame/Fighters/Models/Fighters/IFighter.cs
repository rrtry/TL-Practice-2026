namespace Fighters.Models.Fighters;

public interface IFighter : INamed
{
    public int GetCurrentHealth();
    public int GetMaxHealth();
    public int CalculateDamage();
    public int CalculateArmor();
    public int GetInitiative();
    public DamageStats TakeDamage( Random random, IFighter attacker );
}