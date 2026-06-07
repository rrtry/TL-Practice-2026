using Fighters.Models.Fighters;
using Fighters.Services.Environment;
using Fighters.Factories;
using Moq;

namespace Fighters.Tests.Factories;

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
            .Returns( "" )       // пустое имя -> ошибка
            .Returns( "   " )    // пробелы -> ошибка
            .Returns( "Legion" ) // корректное имя
            .Returns( "0" )      // выбор расы (Human)
            .Returns( "0" )      // выбор класса (LegionSoldierClass)
            .Returns( "0" )      // выбор оружия (Fists)
            .Returns( "0" );     // выбор брони (CaesarArmor)

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

    [Fact]
    public void CreateFighter_InvalidRaceSelection_RetriesUntilValid()
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        env.SetupSequence( e => e.ReadLine() )
            .Returns( "Hero" )
            .Returns( "abc" ) // не число
            .Returns( "-1" )  // < 0
            .Returns( "3" )   // > 2 (максимальный индекс для рас)
            .Returns( "1" )   // корректно (Ghoul)
            .Returns( "0" )   // класс
            .Returns( "0" )   // оружие
            .Returns( "0" );  // броня

        env.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        env.Setup( e => e.Write( It.IsAny<string>() ) );

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( "Hero", fighter.Name );
        // Проверяем, что сообщение об ошибке выбора расы выводилось 3 раза
        env.Verify( e => e.WriteLine( "Введите число от 0 до 2." ), Times.Exactly( 3 ) );

        // Ghoul health 110, damage 6, armor 2, initiative 8
        // LegionSoldierClass health 30, damage 12, armor 6, initiative 12
        // Fists damage 8, CaesarArmor armor 10
        Assert.Equal( 110 + 30, fighter.GetMaxHealth() );
        Assert.Equal( 6 + 12 + 8, fighter.CalculateDamage() );
        Assert.Equal( 2 + 6 + 10, fighter.CalculateArmor() );
        Assert.Equal( 8 + 12, fighter.GetInitiative() );
    }

    [Fact]
    public void CreateFighter_InvalidClassSelection_RetriesUntilValid()
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        env.SetupSequence( e => e.ReadLine() )
            .Returns( "Knight" )
            .Returns( "0" )     // раса Human
            .Returns( "" )      // пустой ввод (не число)
            .Returns( "4" )     // > 3 (макс индекс для классов)
            .Returns( "2" )     // корректно: BrotherhoodKnight
            .Returns( "0" )     // оружие
            .Returns( "0" );    // броня

        env.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        env.Setup( e => e.Write( It.IsAny<string>() ) );

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( "Knight", fighter.Name );

        // Дважды ошибка выбора класса (пустая строка и 4)
        env.Verify( e => e.WriteLine( "Введите число от 0 до 3." ), Times.Exactly( 2 ) );
        // Характеристики Human + BrotherhoodKnight + Fists + CaesarArmor
        Assert.Equal( 100 + 40, fighter.GetMaxHealth() );
        Assert.Equal( 5 + 8 + 8, fighter.CalculateDamage() );
        Assert.Equal( 2 + 5 + 10, fighter.CalculateArmor() );
        Assert.Equal( 10 + 7, fighter.GetInitiative() );
    }

    [Fact]
    public void CreateFighter_InvalidWeaponSelection_RetriesUntilValid()
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        env.SetupSequence( e => e.ReadLine() )
            .Returns( "Warrior" )
            .Returns( "0" )     // раса
            .Returns( "0" )     // класс
            .Returns( "10" )    // > 2 (максимум для оружия)
            .Returns( "1" )     // корректно: Axe
            .Returns( "0" );    // броня

        env.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        env.Setup( e => e.Write( It.IsAny<string>() ) );

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( "Warrior", fighter.Name );
        env.Verify( e => e.WriteLine( "Введите число от 0 до 2." ), Times.Once );
        // Human + LegionSoldier + Axe (20) + CaesarArmor
        Assert.Equal( 5 + 12 + 20, fighter.CalculateDamage() );
    }

    [Fact]
    public void CreateFighter_InvalidArmorSelection_RetriesUntilValid()
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        env.SetupSequence( e => e.ReadLine() )
            .Returns( "Tank" )
            .Returns( "0" )  // раса
            .Returns( "0" )  // класс
            .Returns( "0" )  // оружие
            .Returns( "-5" ) // < 0
            .Returns( "4" )  // > 3 (макс индекс для брони)
            .Returns( "2" ); // корректно: CombatArmor

        env.Setup( e => e.WriteLine( It.IsAny<string>() ) );
        env.Setup( e => e.Write( It.IsAny<string>() ) );

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( "Tank", fighter.Name );
        // Две ошибки для брони
        env.Verify( e => e.WriteLine( "Введите число от 0 до 3." ), Times.Exactly( 2 ) );
        // броня: Human(2) + LegionSoldier(6) + CombatArmor(15) = 23
        Assert.Equal( 2 + 6 + 15, fighter.CalculateArmor() );
    }
}