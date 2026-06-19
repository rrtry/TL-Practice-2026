using Fighters.Models.Fighters;
using Fighters.Services.Arena;
using Fighters.Services.Environment;
using Fighters.Services.Randomization;
using Fighters.Tests.Factories;
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
        envMock.Verify( e => e.WriteLine( "Боец TestFighter добавлен на арену!" ), Times.Once );
        Assert.Contains( fighterMock.Object, arena.Fighters );
    }

    [Fact]
    public void ListFighters_EmptyArena_WritesZeroCount()
    {
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();
        var arena = new ArenaService( envMock.Object, randMock.Object );

        arena.ListFighters();

        envMock.Verify( e => e.WriteLine( "\nКол-во бойцов: 0" ), Times.Once );
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
        envMock.Verify( e => e.Write( "Ввёдите номер бойца: " ), Times.Once );
        envMock.Verify( e => e.WriteLine( "Боец под номером 1 удалён" ), Times.Once );

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
        envMock.Verify( e => e.WriteLine( "\nКол-во бойцов: 2" ), Times.Once );
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
        envMock.Verify( e => e.WriteLine( "Добавьте как минимум 2-ух бойцов на арену" ), Times.Once );
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Раунд" ) ) ), Times.Never );
    }

    [Fact]
    public void SimulateBattle_TwoFighters_OneWins()
    {
        // Arrange
        var envMock = new Mock<IEnvironmentService>();
        var randMock = new Mock<IRandomService>();

        // выбор противника всегда первый (индекс 0)
        randMock.Setup( r => r.Next( It.IsAny<int>() ) ).Returns( 0 );
        SetupFixedNonCriticalDamage( randMock, 0.5 ); // множитель урона 0.95, не крит

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
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Раунд 1" ) ) ), Times.Once );
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "A атакует B" ) ) ), Times.Once );
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "B погибает!" ) ) ), Times.Once );
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "A выживает и побеждает!" ) ) ), Times.Once );
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
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "C погибает!" ) ) ), Times.Once );
        // C ни разу не атаковал (был убит до своего хода)
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "C атакует" ) ) ), Times.Never );
        // B атакует как минимум один раз
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "B атакует" ) ) ), Times.AtLeastOnce );

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

        // Расчеты урона с множителем 0.98:
        // A -> B: baseDamage = max(0,40-30)=10, 10 * 0.98 = 9
        // B -> A: baseDamage = max(0,15-5)=10,  10 * 0.98 = 9
        // Здоровье B: 50 -> 41 -> 32 -> 23 -> 14 -> 5 -> -4 (смерть на 6-м раунде)
        // Здоровье A: 100 -> 91 -> 82 -> 73 -> 64 -> 55 (после 5 раундов, т.к. в 6-м B не ходит)

        Assert.Equal( 0, fighterB.GetCurrentHealth() );
        Assert.Equal( 55, fighterA.GetCurrentHealth() );

        envMock.Verify( e => e.WriteLine( It.Is<string>( s =>
            s.Contains( "A атакует B и наносит 9 урона" ) ) ), Times.Exactly( 6 ) ); // 6 раз, пока B не умрёт
        envMock.Verify( e => e.WriteLine( It.Is<string>( s =>
            s.Contains( "B атакует A и наносит 9 урона" ) ) ), Times.Exactly( 5 ) ); // 5 раз (в 6-м раунде B уже мёртв)

        // Итоговое объявление победителя
        envMock.Verify( e => e.WriteLine( It.Is<string>( s =>
            s.Contains( "A выживает и побеждает!" ) ) ), Times.Once );
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

        // Урон 8 за раунд (10*0.8), здоровье 10000
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Лимит раундов исчерпан." ) ) ), Times.Once );
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
        envMock.Verify( e => e.WriteLine( "Введите число от 1 до 2." ), Times.Exactly( 3 ) );
        envMock.Verify( e => e.WriteLine( It.Is<string>( s => s.StartsWith( "Боец под номером" ) ) ), Times.Once );
        Assert.Single( arena.Fighters );
    }

    private static void SetupFixedNonCriticalDamage( Mock<IRandomService> randMock, double factor = 0.5 )
    {
        randMock.Setup( r => r.NextDouble() ).Returns( factor );
    }
}