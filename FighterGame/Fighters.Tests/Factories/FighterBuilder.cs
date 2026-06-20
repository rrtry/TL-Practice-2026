using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;

namespace Fighters.Tests.Factories;

public class FighterBuilder
{
    private readonly Mock<IRace> _raceMock = new Mock<IRace>();
    private readonly Mock<IClass> _classMock = new Mock<IClass>();
    private readonly Mock<IWeapon> _weaponMock = new Mock<IWeapon>();
    private readonly Mock<IArmor> _armorMock = new Mock<IArmor>();
    private string _name = "TestFighter";

    public FighterBuilder WithName( string name )
    {
        _name = name;
        return this;
    }

    public FighterBuilder WithRaceStats( int health = 0, int damage = 0, int armor = 0, int initiative = 0 )
    {
        _raceMock.Setup( r => r.Health ).Returns( health );
        _raceMock.Setup( r => r.Damage ).Returns( damage );
        _raceMock.Setup( r => r.Armor ).Returns( armor );
        _raceMock.Setup( r => r.Initiative ).Returns( initiative );

        return this;
    }

    public FighterBuilder WithClassStats( int health = 0, int damage = 0, int armor = 0, int initiative = 0 )
    {
        _classMock.Setup( c => c.Health ).Returns( health );
        _classMock.Setup( c => c.Damage ).Returns( damage );
        _classMock.Setup( c => c.Armor ).Returns( armor );
        _classMock.Setup( c => c.Initiative ).Returns( initiative );

        return this;
    }

    public FighterBuilder WithWeaponDamage( int damage )
    {
        _weaponMock.Setup( w => w.Damage ).Returns( damage );
        return this;
    }

    public FighterBuilder WithArmorValue( int armor )
    {
        _armorMock.Setup( a => a.Armor ).Returns( armor );
        return this;
    }

    public IFighter Build()
    {
        return new Fighter(
            _name,
            _raceMock.Object,
            _classMock.Object,
            _weaponMock.Object,
            _armorMock.Object
        );
    }
}