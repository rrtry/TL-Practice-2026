using Fighters.Models.Armors;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;

public interface IFighter : IDisplayable
{
    public int GetCurrentHealth();
    public int GetMaxHealth();
    public int CalculateDamage();
    public int CalculateArmor();
    public int GetInitiative();

    public void SetArmor( IArmor armor );
    public void SetWeapon( IWeapon weapon );

    public void TakeDamage( int damage );
    public DamageStats TakeDamage( Random random, IFighter attacker );
}