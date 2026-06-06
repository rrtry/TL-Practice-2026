using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Fighters.Services.Randomization;
using Moq;

namespace Fighters.Tests.Models.Fighters;

public class FighterTests
{

    [Fact]
    public void GetMaxHealth_ReturnsSumOfRaceAndClassHealth()
    {
        // Arrange
        var race = new Mock<IRace>();
        race.Setup( r => r.Health ).Returns( 100 );
        var fighterClass = new Mock<IClass>();
        fighterClass.Setup( c => c.Health ).Returns( 50 );

        var fighter = new Fighter( "Test", race.Object, fighterClass.Object,
            Mock.Of<IWeapon>(), Mock.Of<IArmor>() );

        // Act
        int maxHealth = fighter.GetMaxHealth();

        // Assert
        Assert.Equal( 150, maxHealth );
    }

    [Fact]
    public void CalculateDamage_ReturnsSumOfRaceClassAndWeaponDamage()
    {
        // Arrange
        var race = new Mock<IRace>();
        race.Setup( r => r.Damage ).Returns( 5 );
        var fighterClass = new Mock<IClass>();
        fighterClass.Setup( c => c.Damage ).Returns( 10 );
        var weapon = new Mock<IWeapon>();
        weapon.Setup( w => w.Damage ).Returns( 15 );

        var fighter = new Fighter( "Test", race.Object, fighterClass.Object,
            weapon.Object, Mock.Of<IArmor>() );

        // Act
        int damage = fighter.CalculateDamage();

        // Assert
        Assert.Equal( 30, damage );
    }

    [Fact]
    public void CalculateArmor_ReturnsSumOfRaceClassAndArmor()
    {
        // Arrange
        var race = new Mock<IRace>();
        race.Setup( r => r.Armor ).Returns( 2 );
        var fighterClass = new Mock<IClass>();
        fighterClass.Setup( c => c.Armor ).Returns( 5 );
        var armor = new Mock<IArmor>();
        armor.Setup( a => a.Armor ).Returns( 10 );

        var fighter = new Fighter( "Test", race.Object, fighterClass.Object,
            Mock.Of<IWeapon>(), armor.Object );

        // Act
        int armorValue = fighter.CalculateArmor();

        // Assert
        Assert.Equal( 17, armorValue );
    }

    [Fact]
    public void GetInitiative_ReturnsSumOfRaceAndClassInitiative()
    {
        // Arrange
        var race = new Mock<IRace>();
        race.Setup( r => r.Initiative ).Returns( 8 );
        var fighterClass = new Mock<IClass>();
        fighterClass.Setup( c => c.Initiative ).Returns( 7 );

        var fighter = new Fighter( "Test", race.Object, fighterClass.Object,
            Mock.Of<IWeapon>(), Mock.Of<IArmor>() );

        // Act
        int initiative = fighter.GetInitiative();

        // Assert
        Assert.Equal( 15, initiative );
    }

    [Fact]
    public void TakeDamage_WithoutArmor_AppliesFullDamage()
    {
        // Arrange
        var fighter = CreateFighterWithStats( health: 100, armor: 0 );
        var attacker = CreateAttackerWithDamage( 30 );
        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 1.0 )   // множитель 0.8+0.3*1.0 = 1.1 (максимальный)
            .Returns( 0.5 );  // не критический удар

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Assert
        Assert.Equal( 33, stats.Damage ); // 30 * 1.1 = 33
        Assert.False( stats.IsCritical );
        Assert.Equal( 67, fighter.GetCurrentHealth() );
    }

    [Fact]
    public void TakeDamage_WithArmor_ReducesDamageButNotBelowPiercing()
    {
        // Arrange
        var fighter = CreateFighterWithStats( health: 100, armor: 10 );
        var attacker = CreateAttackerWithDamage( 20 );
        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 0.8 )   // множитель 0.8+0.3*0.8 = 1.04
            .Returns( 0.5 );  // не критический

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Act - расчет:
        // baseDamage = max(0, 20 - 10) = 10
        // piercingDamage = 20*0.1 = 2 -> baseDamage = max( 10, 2) = 10
        // randomDamage = 10 * 1.04 = 10 (целое)
        // finalDamage = 10 (не крит)
        int expectedDamage = ( int )( Math.Max( 0, 20 - 10 ) * 1.04 );
        Assert.Equal( expectedDamage, stats.Damage );
        Assert.Equal( 100 - expectedDamage, fighter.GetCurrentHealth() );
    }

    [Fact]
    public void TakeDamage_PiercingDamage_WhenArmorExceedsAttack()
    {
        // Arrange
        var fighter = CreateFighterWithStats( health: 100, armor: 50 );
        var attacker = CreateAttackerWithDamage( 20 );
        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 0.8 )
            .Returns( 0.5 );

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Assert: piercingDamage = 2, baseDamage = max(0, 20 - 50) = 0
        // randomDamage = 2 * 1.04 = 2
        int expectedDamage = ( int )( 2 * 1.04 );
        Assert.Equal( expectedDamage, stats.Damage );
        Assert.Equal( 100 - expectedDamage, fighter.GetCurrentHealth() );
    }

    [Fact]
    public void TakeDamage_CriticalHit_DoublesDamage()
    {
        // Arrange
        var fighter = CreateFighterWithStats( health: 100, armor: 0 );
        var attacker = CreateAttackerWithDamage( 20 );
        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 0.9 )   // множитель 0.8+0.3*0.9 = 1.07
            .Returns( 0.05 ); // критический ( < 0.1)

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Assert: baseDamage=20, randomDamage=20*1.07=21, finalDamage=42 (крит)
        Assert.True( stats.IsCritical );
        Assert.Equal( 42, stats.Damage );
        Assert.Equal( 58, fighter.GetCurrentHealth() );
    }

    [Fact]
    public void TakeDamage_HealthDoesNotGoBelowZero()
    {
        // Arrange
        var fighter = CreateFighterWithStats( health: 10, armor: 0 );
        var attacker = CreateAttackerWithDamage( 30 );
        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 0.8 )
            .Returns( 0.5 );

        // Act
        fighter.TakeDamage( random.Object, attacker );

        // Assert
        Assert.Equal( 0, fighter.GetCurrentHealth() );
    }

    private IFighter CreateFighterWithStats( int health, int armor )
    {
        var race = new Mock<IRace>();
        race.Setup( r => r.Health ).Returns( health );
        race.Setup( r => r.Armor ).Returns( 0 );
        race.Setup( r => r.Damage ).Returns( 0 );
        race.Setup( r => r.Initiative ).Returns( 0 );

        var fighterClass = new Mock<IClass>();
        fighterClass.Setup( c => c.Health ).Returns( 0 );
        fighterClass.Setup( c => c.Armor ).Returns( armor );
        fighterClass.Setup( c => c.Damage ).Returns( 0 );
        fighterClass.Setup( c => c.Initiative ).Returns( 0 );

        return new Fighter( "Test", race.Object, fighterClass.Object,
            Mock.Of<IWeapon>(), Mock.Of<IArmor>() );
    }

    private IFighter CreateAttackerWithDamage( int damage )
    {
        var race = new Mock<IRace>();
        race.Setup( r => r.Damage ).Returns( 0 );

        var fighterClass = new Mock<IClass>();
        fighterClass.Setup( c => c.Damage ).Returns( damage );

        var weapon = new Mock<IWeapon>();
        weapon.Setup( w => w.Damage ).Returns( 0 );

        return new Fighter( "Attacker", race.Object, fighterClass.Object,
            weapon.Object, Mock.Of<IArmor>() );
    }
}