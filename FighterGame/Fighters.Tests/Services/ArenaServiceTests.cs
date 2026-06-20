using Fighters.Models.Fighters;
using Fighters.Services.Arena;
using Fighters.Services.Environment;
using Fighters.Services.Randomization;
using Fighters.Tests.Factories;
using Fighters.Utils;
using Moq;

namespace Fighters.Tests.Services;

public class ArenaServiceTests
{
    [Fact]
    public void AddFighter_ValidFighter_WritesMessageAndAddsToList()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var fighterMock = new Mock<IFighter>();
        fighterMock.Setup( f => f.Name ).Returns( "TestFighter" );

        // Act
        arena.AddFighter( fighterMock.Object );

        // Assert
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaFighterAdded( fighterMock.Object.Name ) ), Times.Once );
        Assert.Contains( fighterMock.Object, arena.Fighters );
    }

    [Fact]
    public void ListFighters_EmptyArena_WritesZeroCount()
    {
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        arena.ListFighters();

        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaFightersCount( 0 ) ), Times.Once );
        envMock.Verify( e => e.Write( It.IsAny<string>() ), Times.Never );
    }

    [Fact]
    public void GetFighters_ReturnsCurrentFightersInOrder()
    {
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var f1 = new Mock<IFighter>().Object;
        var f2 = new Mock<IFighter>().Object;

        arena.AddFighter( f1 );
        arena.AddFighter( f2 );

        var fighters = arena.Fighters;
        Assert.Equal( 2, fighters.Count );
        Assert.Same( f1, fighters[ 0 ] );
        Assert.Same( f2, fighters[ 1 ] );
    }

    [Fact]
    public void RemoveFighter_ValidIndex_RemovesFighterAndWritesMessages()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();

        // Последовательность ввода: пользователь вводит "1" (удалить первого бойца)
        envMock.SetupSequence( e => e.ReadLine() ).Returns( "1" );
        envMock.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        envMock.Setup( e => e.Write( It.IsAny<string>() ) );

        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var fighter1 = new Mock<IFighter>().Object;
        var fighter2 = new Mock<IFighter>().Object;

        arena.AddFighter( fighter1 );
        arena.AddFighter( fighter2 );

        // Act
        arena.RemoveFighter();

        // Assert
        envMock.Verify( e => e.Write( ApplicationMessages.ArenaPromptRemoveFighter ), Times.Once );
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaFighterRemoved( 1 ) ), Times.Once );

        var fighters = arena.Fighters;
        Assert.Single( fighters );
        Assert.DoesNotContain( fighter1, fighters );
    }

    [Fact]
    public void ListFighters_WithMultipleFighters_WritesAllFighterInfo()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var fighter1 = new Mock<IFighter>();
        fighter1.Setup( f => f.ToString() ).Returns( "Fighter1Info" );

        var fighter2 = new Mock<IFighter>();
        fighter2.Setup( f => f.ToString() ).Returns( "Fighter2Info" );

        arena.AddFighter( fighter1.Object );
        arena.AddFighter( fighter2.Object );

        // Act
        arena.ListFighters();

        // Assert
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaFightersCount( 2 ) ), Times.Once );
        envMock.Verify( e => e.Write( "1 - " ), Times.Once );
        envMock.Verify( e => e.Write( "Fighter1Info" ), Times.Once );
        envMock.Verify( e => e.Write( "2 - " ), Times.Once );
        envMock.Verify( e => e.Write( "Fighter2Info" ), Times.Once );
    }

    [Fact]
    public void SimulateBattle_NotEnoughFighters_WritesWarningAndDoesNotStart()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var fighter = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 100, damage: 10, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( fighter );

        // Act
        arena.SimulateBattle();

        // Assert
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaNotEnoughFighters ), Times.Once );
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.ArenaRoundHeader( 1 ) ) ) ), Times.Never );
    }

    [Fact]
    public void SimulateBattle_TwoFighters_OneWins()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        SetupFixedNonCriticalDamage( randMock, 0.5 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var fighterA = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 100, damage: 50, armor: 0, initiative: 10 )
            .Build();

        var fighterB = new FighterBuilder()
            .WithName( "B" )
            .WithRaceStats( health: 30, damage: 10, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( fighterA );
        arena.AddFighter( fighterB );

        // Act
        arena.SimulateBattle();

        // Assert
        // Ожидаемый урон от A = 50 * 0.95 = 47 (B умирает)
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaRoundHeader( 1 ) ), Times.Once );
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaAttackMessage( fighterA.Name, fighterB.Name, 47, false ) ), Times.Once );
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaFighterDied( fighterB.Name ) ), Times.Once );
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaWinner( fighterA.Name ) ), Times.Once );
    }

    [Fact]
    public void GetFighters_InitiallyEmpty_ReturnsEmptyCollection()
    {
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var fighters = arena.Fighters;
        Assert.Empty( fighters );
    }

    [Fact]
    public void SimulateBattle_FighterKilledBeforeHisTurn_DoesNotAct()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 ); // Явно 0, а не default int = 0 по умолчанию
        randMock.SetupSequence( r => r.Next( It.IsAny<int>() ) )
                .Returns( 1 )
                .Returns( 0 );
        randMock.Setup( r => r.NextDouble() ).Returns( 0.6 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var fighterA = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 100, damage: 50, armor: 0, initiative: 10 )
            .Build();

        var fighterB = new FighterBuilder()
            .WithName( "B" )
            .WithRaceStats( health: 80, damage: 20, armor: 0, initiative: 7 )
            .Build();

        var fighterC = new FighterBuilder()
            .WithName( "C" )
            .WithRaceStats( health: 40, damage: 30, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( fighterA );
        arena.AddFighter( fighterB );
        arena.AddFighter( fighterC );

        // Act
        arena.SimulateBattle();

        // Assert
        // C погибает от удара A в первом раунде
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.ArenaFighterDied( fighterC.Name ) ) ) ), Times.Once );
        // C ни разу не атаковал (был убит до своего хода)
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.ArenaFighterAttacks( fighterC.Name ) ) ) ), Times.Never );
        // B атакует как минимум один раз
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.ArenaFighterAttacks( fighterB.Name ) ) ) ), Times.AtLeastOnce );

        // C мёртв
        Assert.Equal( 0, fighterC.GetCurrentHealth() );
    }

    [Fact]
    public void SimulateBattle_TargetSelection_UsesRandomCorrectly()
    {
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        randMock.Setup( r => r.NextDouble() ).Returns( 0.6 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var f1 = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 50, damage: 20, armor: 0, initiative: 10 )
            .Build();

        var f2 = new FighterBuilder()
            .WithName( "B" )
            .WithRaceStats( health: 50, damage: 20, armor: 0, initiative: 7 )
            .Build();

        var f3 = new FighterBuilder()
            .WithName( "C" )
            .WithRaceStats( health: 50, damage: 20, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( f1 );
        arena.AddFighter( f2 );
        arena.AddFighter( f3 );

        arena.SimulateBattle();

        // В начале каждого раунда, пока противников >=2, Next(2) вызывается для каждого живого бойца.
        randMock.Verify( r => r.Next( 2 ), Times.AtLeast( 3 ) );
        // Когда остаётся один противник, диапазон сужается до 1.
        randMock.Verify( r => r.Next( 1 ), Times.AtLeastOnce );
    }

    [Fact]
    public void SimulateBattle_AccurateHealthReduction_WithArmor()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        // Каждый вызов Next (выбор противника) всегда возвращает 0
        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        // NextDouble возвращает 0.6 -> множитель = 0.8 + 0.6 * 0.3 = 0.98
        randMock.Setup( r => r.NextDouble() ).Returns( 0.6 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var fighterA = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 100, damage: 40, armor: 5, initiative: 10 )
            .Build();

        var fighterB = new FighterBuilder()
            .WithName( "B" )
            .WithRaceStats( health: 50, damage: 15, armor: 30, initiative: 5 )
            .Build();

        arena.AddFighter( fighterA );
        arena.AddFighter( fighterB );

        // Act
        arena.SimulateBattle();

        // Assert
        Assert.Equal( 0, fighterB.GetCurrentHealth() );
        Assert.Equal( 55, fighterA.GetCurrentHealth() );

        string fighterAMessage = ApplicationMessages.ArenaAttackMessage( fighterA.Name, fighterB.Name, 9, false );
        string fighterBMessage = ApplicationMessages.ArenaAttackMessage( fighterB.Name, fighterA.Name, 9, false );

        envMock.Verify( e => e.WriteLine( It.Is<string>( s =>
            s.Contains( fighterAMessage ) ) ), Times.Exactly( 6 ) ); // 6 раз, пока B не умрёт
        envMock.Verify( e => e.WriteLine( It.Is<string>( s =>
            s.Contains( fighterBMessage ) ) ), Times.Exactly( 5 ) ); // 5 раз (в 6-м раунде B уже мёртв)

        // Итоговое объявление победителя
        envMock.Verify( e => e.WriteLine( It.Is<string>( s =>
            s.Contains( ApplicationMessages.ArenaWinner( fighterA.Name ) ) ) ), Times.Once );
    }

    [Fact]
    public void SimulateBattle_MaxRoundsExhausted_SeveralSurvivors()
    {
        // Arrange: 3 бойца с большим здоровьем и слабой атакой, бой затянется на 100+ раундов.
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        randMock.Setup( r => r.NextDouble() ).Returns( 0.0 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var fighterA = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 10000, damage: 10, armor: 0, initiative: 10 )
            .Build();

        var fighterB = new FighterBuilder()
            .WithName( "B" )
            .WithRaceStats( health: 10000, damage: 10, armor: 0, initiative: 7 )
            .Build();

        var fighterC = new FighterBuilder()
            .WithName( "C" )
            .WithRaceStats( health: 10000, damage: 10, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( fighterA );
        arena.AddFighter( fighterB );
        arena.AddFighter( fighterC );

        // Act
        arena.SimulateBattle();

        // Проверяем, что C не получил урона и выиграл.
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaMaxRoundsExhausted( fighterC.Name ) ), Times.Once );
        // Все трое живы
        Assert.True( fighterA.GetCurrentHealth() > 0 && fighterB.GetCurrentHealth() > 0 && fighterC.GetCurrentHealth() > 0 );
    }

    [Fact]
    public void RemoveFighter_InvalidThenValidInput_RemovesCorrectly()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        envMock.SetupSequence( e => e.ReadLine() )
               .Returns( "abc" )  // не число -> ошибка
               .Returns( "0" )    // меньше 1 -> ошибка
               .Returns( "3" )    // больше 2 (всего два бойца) -> ошибка
               .Returns( "1" );   // корректный индекс

        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        var f1 = new Mock<IFighter>().Object;
        var f2 = new Mock<IFighter>().Object;

        arena.AddFighter( f1 );
        arena.AddFighter( f2 );

        // Act
        arena.RemoveFighter();

        // Assert: было 3 сообщения об ошибке, затем удаление
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ErrorNumberOutOfRange( 1, 2 ) ), Times.Exactly( 3 ) );
        envMock.Verify( e => e.WriteLine( ApplicationMessages.ArenaFighterRemoved( 1 ) ), Times.Once );
        Assert.Single( arena.Fighters );
    }

    [Fact]
    public void SimulateBattle_HigherInitiativeFighterAttacksFirst()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        randMock.SetupSequence( r => r.NextDouble() )
                .Returns( 0.0 )
                .Returns( 0.5 )
                .Returns( 0.0 )
                .Returns( 0.5 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var fast = new FighterBuilder()
            .WithName( "Fast" )
            .WithRaceStats( health: 100, damage: 100, armor: 0, initiative: 20 )
            .Build();

        var slow = new FighterBuilder()
            .WithName( "Slow" )
            .WithRaceStats( health: 100, damage: 10, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( slow );
        arena.AddFighter( fast );

        // Act
        arena.SimulateBattle();

        // Assert
        var messages = envMock.Invocations
            .Where( i => i.Method.Name == nameof( IEnvironmentService.WriteLine ) )
            .Select( i => i.Arguments[ 0 ]?.ToString() )
            .ToList();

        string fastAttacksMessage = ApplicationMessages.ArenaAttackMessage( fast.Name, slow.Name, 80, false );
        string slowAttacksMessage = ApplicationMessages.ArenaAttackMessage( slow.Name, fast.Name, 8, false );

        Assert.NotNull( messages );
        Assert.Contains( fastAttacksMessage, messages );
        Assert.Contains( slowAttacksMessage, messages );

        int fastIndex = messages.FindIndex( m => m != null && m == fastAttacksMessage );
        int slowIndex = messages.FindIndex( m => m != null && m == slowAttacksMessage );

        Assert.True( fastIndex >= 0 && slowIndex >= 0 && fastIndex < slowIndex );
    }

    [Fact]
    public void SimulateBattle_AfterBattle_FightersListCleared()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        randMock.Setup( r => r.NextDouble() ).Returns( 0.5 );

        var arena = new ArenaService( envMock.Object, randMock.Object );

        var f1 = new FighterBuilder()
            .WithName( "A" )
            .WithRaceStats( health: 100, damage: 100, armor: 0, initiative: 10 )
            .Build();

        var f2 = new FighterBuilder()
            .WithName( "B" )
            .WithRaceStats( health: 50, damage: 10, armor: 0, initiative: 5 )
            .Build();

        arena.AddFighter( f1 );
        arena.AddFighter( f2 );

        // Act
        arena.SimulateBattle();

        // Assert
        Assert.Empty( arena.Fighters );
    }

    [Fact]
    public void SimulateBattle_CriticalHit_WritesCriticalMessageAndDoublesDamage()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        randMock.SetupSequence( r => r.NextDouble() )
                .Returns( 0.8 )
                .Returns( 0.05 ) // крит удар
                .Returns( 0.8 )
                .Returns( 0.5 );

        var arena = new ArenaService( envMock.Object, randMock.Object );
        var attacker = new FighterBuilder()
            .WithName( "Crit" )
            .WithRaceStats( health: 100, damage: 20, armor: 0, initiative: 10 )
            .Build();

        var target = new FighterBuilder()
            .WithName( "Victim" )
            .WithRaceStats( health: 100, damage: 1, armor: 10, initiative: 5 )
            .Build();

        arena.AddFighter( attacker );
        arena.AddFighter( target );

        // Act
        arena.SimulateBattle();

        // Assert
        // После крита урон = 10 * 2 = 20
        string critAttackMessage = ApplicationMessages.ArenaAttackMessage( attacker.Name, target.Name, 20, true );
        envMock.Verify( e => e.WriteLine( critAttackMessage ), Times.Once );
    }

    private static void SetupFixedNonCriticalDamage( Mock<IRandomService> randMock, double factor = 0.5 )
    {
        randMock.Setup( r => r.NextDouble() ).Returns( factor );
    }
}