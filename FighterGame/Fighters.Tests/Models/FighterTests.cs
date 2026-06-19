using Fighters.Services.Randomization;
using Fighters.Tests.Factories;
using Moq;

namespace Fighters.Tests.Models;

public class FighterTests
{

    [Fact]
    public void GetMaxHealth_ReturnsSumOfRaceAndClassHealth()
    {
        // Arrange
        var fighter = new FighterBuilder()
            .WithRaceStats( health: 100 )
            .WithClassStats( health: 50 )
            .Build();

        // Act
        int maxHealth = fighter.GetMaxHealth();

        // Assert
        Assert.Equal( 150, maxHealth );
    }

    [Fact]
    public void CalculateDamage_ReturnsSumOfRaceClassAndWeaponDamage()
    {
        // Arrange
        var fighter = new FighterBuilder()
            .WithRaceStats( damage: 5 )
            .WithClassStats( damage: 10 )
            .WithWeaponDamage( 15 )
            .Build();

        // Act
        int damage = fighter.CalculateDamage();

        // Assert
        Assert.Equal( 30, damage );
    }

    [Fact]
    public void CalculateArmor_ReturnsSumOfRaceClassAndArmor()
    {
        // Arrange
        var fighter = new FighterBuilder()
            .WithRaceStats( armor: 2 )
            .WithClassStats( armor: 5 )
            .WithArmorValue( 10 )
            .Build();

        // Act
        int armorValue = fighter.CalculateArmor();

        // Assert
        Assert.Equal( 17, armorValue );
    }

    [Fact]
    public void GetInitiative_ReturnsSumOfRaceAndClassInitiative()
    {
        // Arrange
        var fighter = new FighterBuilder()
            .WithRaceStats( initiative: 8 )
            .WithClassStats( initiative: 7 )
            .Build();

        // Act
        int initiative = fighter.GetInitiative();

        // Assert
        Assert.Equal( 15, initiative );
    }

    [Fact]
    public void TakeDamage_WithoutArmor_AppliesFullDamage()
    {
        // Arrange
        var fighter = new FighterBuilder()
            .WithRaceStats( health: 100 )
            .Build();

        var attacker = new FighterBuilder()
            .WithName( "Attacker" )
            .WithWeaponDamage( 30 )
            .Build();

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
        var fighter = new FighterBuilder()
            .WithRaceStats( health: 100 )
            .WithClassStats( armor: 10 )
            .Build();

        var attacker = new FighterBuilder()
            .WithName( "Attacker" )
            .WithWeaponDamage( 20 )
            .Build();

        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 0.8 )   // множитель 0.8+0.3*0.8 = 1.04
            .Returns( 0.5 );  // не критический

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Assert - расчет:
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
        var fighter = new FighterBuilder()
            .WithRaceStats( health: 100 )
            .WithClassStats( armor: 50 )
            .Build();

        var attacker = new FighterBuilder()
            .WithName( "Attacker" )
            .WithWeaponDamage( 20 )
            .Build();

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
        var fighter = new FighterBuilder()
            .WithRaceStats( health: 100 )
            .Build();

        var attacker = new FighterBuilder()
            .WithName( "Attacker" )
            .WithWeaponDamage( 20 )
            .Build();

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
        var fighter = new FighterBuilder()
            .WithRaceStats( health: 10 )
            .Build();

        var attacker = new FighterBuilder()
            .WithName( "Attacker" )
            .WithWeaponDamage( 30 )
            .Build();

        var random = new Mock<IRandomService>();
        random.SetupSequence( r => r.NextDouble() )
            .Returns( 0.8 )
            .Returns( 0.5 );

        // Act
        fighter.TakeDamage( random.Object, attacker );

        // Assert
        Assert.Equal( 0, fighter.GetCurrentHealth() );
    }
}