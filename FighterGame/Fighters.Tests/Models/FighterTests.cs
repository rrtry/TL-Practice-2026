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
            .Returns( 1.0 )
            .Returns( 0.5 );

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Assert
        Assert.Equal( 33, stats.Damage );
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
            .Returns( 0.0 )
            .Returns( 0.5 ); // не критический

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        // Assert
        const int expectedDamage = 8;
        const int expectedHealth = 92;

        Assert.Equal( expectedDamage, stats.Damage );
        Assert.False( stats.IsCritical );
        Assert.Equal( expectedHealth, fighter.GetCurrentHealth() );
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
            .Returns( 1.0 )
            .Returns( 0.5 );

        // Act
        var stats = fighter.TakeDamage( random.Object, attacker );

        const int expectedDamage = 2;
        const int expectedHealth = 98;

        // Assert
        Assert.Equal( expectedDamage, stats.Damage );
        Assert.False( stats.IsCritical );
        Assert.Equal( expectedHealth, fighter.GetCurrentHealth() );
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
            .Returns( 0.9 )
            .Returns( 0.05 );

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