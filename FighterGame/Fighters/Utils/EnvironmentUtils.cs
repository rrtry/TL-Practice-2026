using Fighters.Services.Environment;

namespace Fighters.Utils;

public static class EnvironmentUtils
{
    public static int ReadIntInRange( IEnvironmentService env, int min, int max )
    {
        while ( true )
        {
            if ( int.TryParse( env.ReadLine(), out int value ) && value >= min && value <= max )
            {
                return value;
            }

            env.WriteLine( $"Введите число от {min} до {max}." );
        }
    }

    public static string ReadString( IEnvironmentService env )
    {
        while ( true )
        {
            string? line = env.ReadLine();
            if ( !string.IsNullOrWhiteSpace( line ) )
            {
                return line;
            }

            env.WriteLine( "Имя не может быть пустым." );
        }
    }
}
