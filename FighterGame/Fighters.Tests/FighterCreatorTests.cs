using Fighters.Models.Fighters;
using Fighters.Services.Environment;
using Moq;

namespace Fighters.Tests;

public class FighterCreatorTests
{
    [Fact]
    public void CreateFighter_ReadsNameAndChoices_ReturnsCorrectFighter()
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        // Последовательность ввода: имя, выбор расы (0), класса (1), оружия (2), брони (3)
        env.SetupSequence( e => e.ReadLine() )
            .Returns( "John" ) // имя
            .Returns( "0" )    // раса Human
            .Returns( "1" )    // класс NCRTrooperClass
            .Returns( "2" )    // оружие Machete
            .Returns( "3" );   // броня NoArmor

        env.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        env.Setup( e => e.Write( It.IsAny<string>() ) );

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( "John", fighter.Name );
        Assert.Equal( 100 + 25, fighter.GetMaxHealth() );       // Human (100) + NCRTrooper (25)
        Assert.Equal( 5 + 10 + 18, fighter.CalculateDamage() ); // Human(5) + NCR(10) + Machete(18) = 33
        Assert.Equal( 2 + 8 + 0, fighter.CalculateArmor() );    // Human(2)+NCR(8)+NoArmor(0)=10
        Assert.Equal( 10 + 10, fighter.GetInitiative() );       // Human(10) + NCR(10) = 20

        // Проверяем, что были вызваны WriteLine для меню выбора
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Выберите расу" ) ) ), Times.Once );
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Выберите класс" ) ) ), Times.Once );
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Выберите оружие" ) ) ), Times.Once );
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( "Выберите броню" ) ) ), Times.Once );
    }

    [Fact]
    public void CreateFighter_InvalidName_RetriesUntilValid()
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        env.SetupSequence( e => e.ReadLine() )
            .Returns( "" )       // 1: пустое имя -> ошибка
            .Returns( "   " )    // 2: пробелы -> ошибка
            .Returns( "Legion" ) // 3: корректное имя
            .Returns( "0" )      // 4: выбор расы (Human)
            .Returns( "0" )      // 5: выбор класса (LegionSoldierClass)
            .Returns( "0" )      // 6: выбор оружия (Fists)
            .Returns( "0" );     // 7: выбор брони (CaesarArmor)

        env.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        env.Setup( e => e.Write( It.IsAny<string>() ) );

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( "Legion", fighter.Name );
        // Два раза была ошибка: пустая строка и пробелы
        env.Verify( e => e.WriteLine( "Имя не может быть пустым." ), Times.Exactly( 2 ) );
    }
}