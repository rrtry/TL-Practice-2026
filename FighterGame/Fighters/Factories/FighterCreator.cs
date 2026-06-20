using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Services.Environment;
using Fighters.Utils;

namespace Fighters.Factories;

public class FighterCreator
{
    private static readonly IReadOnlyList<IRace> Races = new List<IRace> {
        new Human(),
        new Ghoul(),
        new Mutant()
    };

    private static readonly IReadOnlyList<IClass> Classes = new List<IClass> {
        new LegionSoldierClass(),
        new NCRTrooperClass(),
        new BrotherhoodKnight(),
        new MojaveWanderer()
    };

    private static readonly IReadOnlyList<IWeapon> Weapons = new List<IWeapon> {
        new Fists(),
        new Axe(),
        new Machete()
    };

    private static readonly IReadOnlyList<IArmor> Armors = new List<IArmor> {
        new CaesarArmor(),
        new PowerArmor(),
        new CombatArmor(),
        new NoArmor()
    };

    private readonly IEnvironmentService _env;

    public FighterCreator( IEnvironmentService env )
    {
        _env = env;
    }

    public IFighter CreateFighter()
    {
        _env.Write( ApplicationMessages.CreatorCharacterName );
        string name = EnvironmentUtils.ReadString( _env );

        IRace race = SelectRace();
        IClass fighterClass = SelectFighterClass();
        IWeapon weapon = SelectWeapon();
        IArmor armor = SelectArmor();

        return new Fighter( name, race, fighterClass, weapon, armor );
    }

    private IRace SelectRace()
    {
        _env.WriteLine( ApplicationMessages.CreatorCharacterRace );

        IRace race;
        for ( int i = 0; i < Races.Count; i++ )
        {
            race = Races[ i ];
            _env.WriteLine( ApplicationMessages.CreatorRaceOption( i, race.Name, race.Damage, race.Health, race.Armor, race.Initiative ) );
        }

        int raceIndex = EnvironmentUtils.ReadIntInRange( _env, 0, Races.Count - 1 );
        race = Races[ raceIndex ];

        return race;
    }

    private IClass SelectFighterClass()
    {
        _env.WriteLine( ApplicationMessages.CreatorCharacterClass );

        IClass fighterClass;
        for ( int i = 0; i < Classes.Count; i++ )
        {
            fighterClass = Classes[ i ];
            _env.WriteLine( ApplicationMessages.CreatorClassOption( i, fighterClass.Name, fighterClass.Damage, fighterClass.Health, fighterClass.Armor, fighterClass.Initiative ) );
        }

        int classIndex = EnvironmentUtils.ReadIntInRange( _env, 0, Classes.Count - 1 );
        fighterClass = Classes[ classIndex ];

        return fighterClass;
    }

    private IWeapon SelectWeapon()
    {
        _env.WriteLine( ApplicationMessages.CreatorCharacterWeapon );

        IWeapon weapon;
        for ( int i = 0; i < Weapons.Count; i++ )
        {
            weapon = Weapons[ i ];
            _env.WriteLine( ApplicationMessages.CreatorWeaponOption( i, weapon.Name, weapon.Damage ) );
        }

        int weaponIndex = EnvironmentUtils.ReadIntInRange( _env, 0, Weapons.Count - 1 );
        weapon = Weapons[ weaponIndex ];

        return weapon;
    }

    private IArmor SelectArmor()
    {
        _env.WriteLine( ApplicationMessages.CreatorCharacterArmor );

        IArmor armor;
        for ( int i = 0; i < Armors.Count; i++ )
        {
            armor = Armors[ i ];
            _env.WriteLine( ApplicationMessages.CreatorArmorOption( i, armor.Name, armor.Armor ) );
        }

        int armorIndex = EnvironmentUtils.ReadIntInRange( _env, 0, Armors.Count - 1 );
        armor = Armors[ armorIndex ];

        return armor;
    }
}
