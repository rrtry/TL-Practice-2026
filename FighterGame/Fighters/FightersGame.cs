namespace Fighters;

public class FightersGame
{
    private const string GAME_TITLE = "Fallout Arena: ";

    private const string CMD_ADD_FIGHTER = "add-fighter";
    private const string CMD_LIST_FIGHTERS = "list-fighters";
    private const string CMD_REMOVE_FIGHTER = "remove-fighter";

    private const string CMD_PLAY = "play";
    private const string CMD_EXIT = "exit";
    private const string CMD_HELP = "help";

    private readonly static Dictionary<string, string> Commands = new Dictionary<string, string>
    {
        { CMD_ADD_FIGHTER,  "Добавить нового бойца на арену" },
        { CMD_LIST_FIGHTERS, "Вывести список бойцов на арене" },
        { CMD_REMOVE_FIGHTER, "Удалить бойца с арены" },

        { CMD_PLAY, "Начать битву" },
        { CMD_HELP, "Вывести доступные команды" },
        { CMD_EXIT, "Выйти" }
    };

    private readonly IEnvironment _env;
    private Arena _arena;
    private FighterCreator _creator;

    public FightersGame( IEnvironment env )
    {
        _env = env;
        _arena = new Arena( _env );
        _creator = new FighterCreator( _env );
    }

    public void Run()
    {
        _env.WriteLine( GAME_TITLE );
        PrintMenu();

        while ( true )
        {
            string? command = _env.ReadLine()?.Trim().ToLower();
            if ( !ProcessCommand( command ) )
            {
                break;
            }
        }
    }

    private void PrintMenu()
    {
        _env.WriteLine( "\nВведите команду: " );
        foreach ( var (command, description) in Commands )
        {
            _env.WriteLine( $"{command} - {description}" );
        }
    }

    private bool ProcessCommand( string? command )
    {
        switch ( command )
        {
            case CMD_ADD_FIGHTER:
                _arena.AddFighter( _creator.CreateFighter() );
                return true;

            case CMD_LIST_FIGHTERS:
                _arena.ListFighters();
                return true;

            case CMD_REMOVE_FIGHTER:
                _arena.RemoveFighter();
                return true;

            case CMD_PLAY:
                _arena.SimulateBattle();
                return true;

            case CMD_HELP:
                PrintMenu();
                return true;

            case CMD_EXIT:
                return false;

            default:
                _env.WriteLine( "Неизвестная команда. Попробуйте снова." );
                return true;
        }
    }
}
