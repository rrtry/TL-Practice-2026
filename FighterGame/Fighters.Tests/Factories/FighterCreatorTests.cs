using Fighters.Models.Fighters;
using Fighters.Services.Environment;
using Fighters.Factories;
using Moq;
using Fighters.Utils;

namespace Fighters.Tests.Factories;

public class FighterCreatorTests
{
    [Theory]
    [MemberData( nameof( ValidInputsData ) )]
    public void CreateFighter_WithValidInputs_BuildsFighterMatchingChoices(
        IReadOnlyList<string> allInputs,
        string expectedName,
        int expectedMaxHealth,
        int expectedDamage,
        int expectedArmor,
        int expectedInitiative )
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        var sequence = env.SetupSequence( e => e.ReadLine() );
        foreach ( var input in allInputs )
        {
            sequence.Returns( input );
        }

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( expectedName, fighter.Name );
        Assert.Equal( expectedMaxHealth, fighter.GetMaxHealth() );
        Assert.Equal( expectedDamage, fighter.CalculateDamage() );
        Assert.Equal( expectedArmor, fighter.CalculateArmor() );
        Assert.Equal( expectedInitiative, fighter.GetInitiative() );

        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.CreatorCharacterRace ) ) ), Times.Once );
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.CreatorCharacterClass ) ) ), Times.Once );
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.CreatorCharacterWeapon ) ) ), Times.Once );
        env.Verify( e => e.WriteLine( It.Is<string>( s => s.Contains( ApplicationMessages.CreatorCharacterArmor ) ) ), Times.Once );
    }

    public static TheoryData<string[], string, int, int, int, int> ValidInputsData()
    {
        var data = new TheoryData<string[], string, int, int, int, int>();

        // 1: Human + LegionSoldierClass + Fists + CaesarArmor
        data.Add(
            [ "John", "0", "0", "0", "0" ],
            "John",
            // Health: Human 100 + LegionSoldier 30
            100 + 30,
            // Damage: Human 5 + LegionSoldier 12 + Fists 8
            5 + 12 + 8,
            // Armor: Human 2 + LegionSoldier 6 + CaesarArmor 10
            2 + 6 + 10,
            // Initiative: Human 10 + LegionSoldier 12
            10 + 12
        );

        // 2: Ghoul + NCRTrooperClass + Axe + PowerArmor
        data.Add(
            [ "Lucy", "1", "1", "1", "1" ],
            "Lucy",
            // Health: Ghoul 110 + NCRTrooper 25
            110 + 25,
            // Damage: Ghoul 6 + NCRTrooper 10 + Axe 20
            6 + 10 + 20,
            // Armor: Ghoul 2 + NCRTrooper 8 + PowerArmor 20
            2 + 8 + 20,
            // Initiative: Ghoul 8 + NCRTrooper 10
            8 + 10
        );

        // 3: Mutant + BrotherhoodKnight + Machete + NoArmor
        data.Add(
            [ "Marcus", "2", "2", "2", "3" ],
            "Marcus",
            // Health: Mutant 120 + BrotherhoodKnight 40
            120 + 40,
            // Damage: Mutant 15 + Knight 8 + Machete 18
            15 + 8 + 18,
            // Armor: Mutant 8 + Knight 5 + NoArmor 0
            8 + 5 + 0,
            // Initiative: Mutant 6 + Knight 7
            6 + 7
        );

        // 4: Human + MojaveWanderer + Fists + CombatArmor
        data.Add(
            [ "Alice", "0", "3", "0", "2" ],
            "Alice",
            // Health: Human 100 + Wanderer 20
            100 + 20,
            // Damage: Human 5 + Wanderer 7 + Fists 8
            5 + 7 + 8,
            // Armor: Human 2 + Wanderer 5 + CombatArmor 15
            2 + 5 + 15,
            // Initiative: Human 10 + Wanderer 6
            10 + 6
        );

        // 5: Ghoul + BrotherhoodKnight + Machete + CaesarArmor
        data.Add(
            [ "Nick", "1", "2", "2", "0" ],
            "Nick",
            // Health: Ghoul 110 + Knight 40
            110 + 40,
            // Damage: Ghoul 6 + Knight 8 + Machete 18
            6 + 8 + 18,
            // Armor: Ghoul 2 + Knight 5 + CaesarArmor 10
            2 + 5 + 10,
            // Initiative: Ghoul 8 + Knight 7
            8 + 7
        );

        return data;
    }

    [Theory]
    [MemberData( nameof( InvalidNumericInputsData ) )]
    public void CreateFighter_InvalidNumericInputs_RetriesUntilValid(
        IReadOnlyList<string> allInputs,
        string expectedErrorMessage,
        int expectedErrorCount,
        int expectedMaxHealth,
        int expectedDamage,
        int expectedArmor,
        int expectedInitiative )
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        var sequence = env.SetupSequence( e => e.ReadLine() );

        foreach ( var input in allInputs )
        {
            sequence.Returns( input );
        }

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        env.Verify( e => e.WriteLine( expectedErrorMessage ), Times.Exactly( expectedErrorCount ) );
        Assert.Equal( expectedMaxHealth, fighter.GetMaxHealth() );
        Assert.Equal( expectedDamage, fighter.CalculateDamage() );
        Assert.Equal( expectedArmor, fighter.CalculateArmor() );
        Assert.Equal( expectedInitiative, fighter.GetInitiative() );
    }

    public static TheoryData<IReadOnlyList<string>, string, int, int, int, int, int> InvalidNumericInputsData()
    {
        var data = new TheoryData<IReadOnlyList<string>, string, int, int, int, int, int>();

        // Тест на расу
        data.Add(
            [ "Hero", "abc", "-1", "3", "1", "0", "0", "0" ],
            ApplicationMessages.ErrorNumberOutOfRange( 0, 2 ),
            3,
            110 + 30,   // Ghoul health 110 + LegionSoldier health 30
            6 + 12 + 8, // Ghoul 6 + LegionSoldier 12 + Fists 8
            2 + 6 + 10, // Ghoul 2 + LegionSoldier 6 + CaesarArmor 10
            8 + 12      // Ghoul 8 + LegionSoldier 12
        );

        // Тест на класс
        data.Add(
            [ "Knight", "0", "", "4", "2", "0", "0" ],
            ApplicationMessages.ErrorNumberOutOfRange( 0, 3 ),
            2,
            100 + 40,   // Human 100 + Knight 40
            5 + 8 + 8,  // Human 5 + Knight 8 + Fists 8
            2 + 5 + 10, // Human 2 + Knight 5 + CaesarArmor 10
            10 + 7      // Human 10 + Knight 7
        );

        // Тест на оружие
        data.Add(
            [ "Warrior", "0", "0", "10", "1", "0" ],
            ApplicationMessages.ErrorNumberOutOfRange( 0, 2 ),
            1,
            100 + 30,    // Human 100 + LegionSoldier 30
            5 + 12 + 20, // Human 5 + LegionSoldier 12 + Axe 20
            2 + 6 + 10,  // Human 2 + LegionSoldier 6 + CaesarArmor 10
            10 + 12      // Human + LegionSoldier
        );

        // Тест на броню
        data.Add(
            [ "Tank", "0", "0", "0", "-5", "4", "2" ],
            ApplicationMessages.ErrorNumberOutOfRange( 0, 3 ),
            2,
            100 + 30,   // Human + LegionSoldier
            5 + 12 + 8, // Human 5 + LegionSoldier 12 + Fists 8
            2 + 6 + 15, // Human 2 + LegionSoldier 6 + CombatArmor 15
            10 + 12     // Human + LegionSoldier
        );

        return data;
    }

    [Theory]
    [MemberData( nameof( InvalidNameData ) )]
    public void CreateFighter_InvalidName_RetriesUntilValid(
        IReadOnlyList<string> allInputs,
        int expectedErrorCount,
        string expectedFinalName )
    {
        // Arrange
        var env = new Mock<IEnvironmentService>();
        var sequence = env.SetupSequence( e => e.ReadLine() );
        foreach ( var input in allInputs )
        {
            sequence.Returns( input );
        }

        var creator = new FighterCreator( env.Object );

        // Act
        IFighter fighter = creator.CreateFighter();

        // Assert
        Assert.Equal( expectedFinalName, fighter.Name );
        env.Verify( e => e.WriteLine( ApplicationMessages.ErrorEmptyName ), Times.Exactly( expectedErrorCount ) );
    }

    public static TheoryData<string[], int, string> InvalidNameData()
    {
        var data = new TheoryData<string[], int, string>();

        // Однократная ошибка
        data.Add(
            [ "", "Legion", "0", "0", "0", "0" ],
            1,
            "Legion"
        );

        // Однократная ошибка: только пробелы
        data.Add(
            [ "   ", "Hero", "0", "0", "0", "0" ],
            1,
            "Hero"
        );

        // Две ошибки
        data.Add(
            [ "", "   ", "Legion", "0", "0", "0", "0" ],
            2,
            "Legion"
        );

        return data;
    }
}