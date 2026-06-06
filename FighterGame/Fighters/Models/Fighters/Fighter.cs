using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Services.Randomization;

namespace Fighters.Models.Fighters;

public class Fighter : IFighter
{
    private readonly IRace _race;
    private readonly IClass _class;

    private readonly IWeapon _weapon;
    private readonly IArmor _armor;

    private int _currentHealth;

    public string Name { get; }

    public Fighter( string name, IRace race, IClass fighterClass, IWeapon weapon, IArmor armor )
    {
        Name = name;
        _race = race;
        _class = fighterClass;
        _weapon = weapon;
        _armor = armor;
        _currentHealth = GetMaxHealth();
    }

    public int GetMaxHealth() => _race.Health + _class.Health;
    public int GetCurrentHealth() => _currentHealth;
    public int CalculateDamage() => _race.Damage + _class.Damage + _weapon.Damage;
    public int CalculateArmor() => _race.Armor + _class.Armor + _armor.Armor;
    public int GetInitiative() => _race.Initiative + _class.Initiative;

    public DamageStats TakeDamage( IRandomService random, IFighter attacker )
    {
        int attackerDamage = attacker.CalculateDamage();
        int baseDamage = Math.Max( 0, attackerDamage - CalculateArmor() );
        int piercingDamage = ( int )( attackerDamage * 0.1 );

        baseDamage = Math.Max( baseDamage, piercingDamage );

        // Изменение базового урона
        double randomFactor = 0.8 + ( random.NextDouble() * 0.3 );
        int randomDamage = ( int )( baseDamage * randomFactor );

        // Критический урон ( 10% )
        bool isCritical = random.NextDouble() < 0.1;
        int finalDamage = isCritical ? randomDamage * 2 : randomDamage;

        ApplyDamage( finalDamage );

        return new DamageStats( isCritical, finalDamage );
    }

    public override string ToString()
    {
        return $"Боец {Name}:\n" +
               $"Класс: {_class.Name}, Здоровье: {_class.Health}, Урон: {_class.Damage}, Защита: {_class.Armor} Инициатива: {_class.Initiative}\n" +
               $"Раса: {_race.Name}, Здоровье: {_race.Health}, Урон: {_race.Damage}, Защита: {_race.Armor} Инициатива: {_race.Initiative}\n" +
               $"Оружие: {_weapon.Name}, Урон: {_weapon.Damage}\n" +
               $"Броня: {_armor.Name}: Защита: {_armor.Armor}\n\n";
    }

    private void ApplyDamage( int damage )
    {
        _currentHealth = Math.Max( 0, _currentHealth - damage );
    }
}