using Fighters;

class Program
{
    static void Main( string[] args )
    {
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        FightersGame instance = new FightersGame( new ConsoleEnvironment() );
        instance.Run();
    }
}