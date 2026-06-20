namespace Fighters.Utils;

public static class ApplicationMessages
{
    // Arena Messages
    public static string ArenaNotEnoughFighters => "Добавьте как минимум 2-ух бойцов на арену";

    public static string ArenaRoundHeader( int round ) => $"\nРаунд {round}";

    public static string ArenaFighterAdded( string name ) => $"Боец {name} добавлен на арену!";

    public static string ArenaPromptRemoveFighter => "Введите номер бойца: ";

    public static string ArenaFighterRemoved( int index ) => $"Боец под номером {index} удалён";

    public static string ArenaFightersCount( int count ) => $"\nКол-во бойцов: {count}";

    public static string ArenaFighterListItemPrefix( int index ) => $"{index} - ";

    public static string ArenaRoundDamageStats( string fighterName, int dealt, int received ) =>
        $"{fighterName} наносит {dealt} урона, получает {received}";

    public static string ArenaFighterDied( string name ) => $"{name} погибает!";

    public static string ArenaWinner( string name ) => $"\n{name} выживает и побеждает!";

    public static string ArenaMaxRoundsExhausted( string name ) =>
        $"\nЛимит раундов исчерпан. Самым живучим оказался {name}!";

    public static string ArenaFighterAttacks( string fighterName ) => $"{fighterName} атакует";

    public static string ArenaAttackMessage( string attacker, string target, int damage, bool isCritical )
    {
        string message = $"{attacker} атакует {target} и наносит {damage} урона";

        if ( isCritical )
        {
            message += " (критический удар!)";
        }

        return message;
    }

    // Character creator messages
    public static string CreatorCharacterName => "Введите имя персонажа: ";

    public static string CreatorCharacterRace => "Выберите расу:";

    public static string CreatorCharacterClass => "Выберите класс:";

    public static string CreatorCharacterWeapon => "Выберите оружие:";

    public static string CreatorCharacterArmor => "Выберите броню:";

    public static string CreatorRaceOption( int index, string name, int damage, int health, int armor, int initiative ) =>
        $"{index} - {name} (Сила+{damage}, Здоровье {health}, Броня {armor}, Инициатива {initiative})";

    public static string CreatorClassOption( int index, string name, int damage, int health, int armor, int initiative ) =>
        $"{index} - {name} (Сила+{damage}, Здоровье+{health}, Броня+{armor}, Инициатива+{initiative})";

    public static string CreatorWeaponOption( int index, string name, int damage ) =>
        $"{index} - {name} (Урон {damage})";

    public static string CreatorArmorOption( int index, string name, int armor ) =>
        $"{index} - {name} (Защита {armor})";

    // Error messages
    public static string ErrorEmptyName => "Имя не может быть пустым.";

    public static string ErrorNumberOutOfRange( int min, int max ) =>
            $"Введите число от {min} до {max}.";
}