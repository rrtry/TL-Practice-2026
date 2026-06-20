using Fighters;
using Fighters.Services.Environment;
using Fighters.Services.Randomization;

class Program
{
    static void Main( string[] args )
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        FightersGame instance = new FightersGame( new ConsoleEnvironmentService(), new SystemRandomService() );
        instance.Run();
    }
}