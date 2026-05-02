using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Utils;

namespace Fighters;

class FighterCreator
{

    public static readonly IReadOnlyList<IRace> Races = new List<IRace> {
        new Human(),
        new Ghoul(),
        new Mutant()
    };

    public static readonly IReadOnlyList<IClass> Classes = new List<IClass> {
        new LegionSoldierClass(),
        new NCRTrooperClass(),
        new BrotherhoodKnight(),
        new MojaveWanderer()
    };

    public static readonly IReadOnlyList<IWeapon> Weapons = new List<IWeapon> {
        new Fists(),
        new Axe(),
        new Machete()
    };

    public static readonly IReadOnlyList<IArmor> Armors = new List<IArmor> {
        new CaesarArmor(),
        new PowerArmor(),
        new CombatArmor(),
        new NoArmor()
    };

    private IEnvironment _environ;

    public FighterCreator( IEnvironment env )
    {
        _environ = env;
    }

    public IRace SelectRace()
    {
        Console.WriteLine( "Выберите расу:" );

        for ( int i = 0; i < Races.Count; i++ )
        {
            Console.WriteLine( $"{i} - {Races[ i ].Name} (Сила+{Races[ i ].Damage}, Здоровье {Races[ i ].Health}, Броня {Races[ i ].Armor}, Инициатива {Races[ i ].Initiative})" );
        }

        int raceIndex = EnvironmentUtils.ReadIntInRange( _environ, 0, Races.Count - 1 );
        IRace race = Races[ raceIndex ];

        return race;
    }

    private IClass SelectFighterClass()
    {
        Console.WriteLine( "Выберите класс:" );

        for ( int i = 0; i < Classes.Count; i++ )
        {
            Console.WriteLine( $"{i} - {Classes[ i ].Name} (Сила+{Classes[ i ].Damage}, Здоровье+{Classes[ i ].Health}, Броня+{Classes[ i ].Armor}, Инициатива+{Classes[ i ].Initiative})" );
        }

        int classIndex = EnvironmentUtils.ReadIntInRange( _environ, 0, Classes.Count - 1 );
        IClass fighterClass = Classes[ classIndex ];

        return fighterClass;
    }

    private IWeapon SelectWeapon()
    {
        Console.WriteLine( "Выберите оружие:" );

        for ( int i = 0; i < Weapons.Count; i++ )
        {
            Console.WriteLine( $"{i} - {Weapons[ i ].Name} (Урон {Weapons[ i ].Damage})" );
        }

        int weaponIndex = EnvironmentUtils.ReadIntInRange( _environ, 0, Weapons.Count - 1 );
        IWeapon weapon = Weapons[ weaponIndex ];

        return weapon;
    }

    private IArmor SelectArmor()
    {
        Console.WriteLine( "Выберите броню:" );

        for ( int i = 0; i < Armors.Count; i++ )
        {
            Console.WriteLine( $"{i} - {Armors[ i ].Name} (Защита {Armors[ i ].Armor})" );
        }

        int armorIndex = EnvironmentUtils.ReadIntInRange( _environ, 0, Armors.Count - 1 );
        IArmor armor = Armors[ armorIndex ];

        return armor;
    }

    public IFighter CreateFighter()
    {
        Console.Write( "Введите имя персонажа: " );
        string name = EnvironmentUtils.ReadString( _environ );

        IRace race = SelectRace();
        IClass fighterClass = SelectFighterClass();
        IWeapon weapon = SelectWeapon();
        IArmor armor = SelectArmor();

        return new Fighter( name, race, fighterClass, weapon, armor );
    }
}
