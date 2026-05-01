using Fighters.Models.Armors;
using Fighters.Models.Races;
using Fighters.Models.Weapons;

namespace Fighters.Models.Fighters;
public class Knight : IFighter
{
    private readonly IRace _race;
    private IArmor _armor = new NoArmor();
    private IWeapon _weapon = new Firsts();

    private int _currentHealth;

    public string Name { get; private set; }

    public Knight( string name, IRace race )
    {
        Name = name;
        _race = race;

        _currentHealth = GetMaxHealth();
    }

    public int GetCurrentHealth() => _currentHealth;

    public int GetMaxHealth() => _race.Health;

    public int CalculateDamage() => _weapon.Damage + _race.Damage;

    public int CalculateArmor() => _armor.Armor + _race.Armor;

    public void SetArmor( IArmor armor )
    {
        _armor = armor;
    }

    public void SetWeapon( IWeapon weapon )
    {
        _weapon = weapon;
    }

    public void TakeDamage( int damage )
    {
        int newHealth = _currentHealth - damage;
        if ( newHealth < 0 )
        {
            newHealth = 0;
        }

        _currentHealth = newHealth;
    }
}