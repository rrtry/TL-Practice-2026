namespace Fighters.Services.Environment;

public class ConsoleEnvironmentService : IEnvironmentService
{
    public void Write( string message )
    {
        Console.Write( message );
    }

    public void WriteLine( string message )
    {
        Console.WriteLine( message );
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
