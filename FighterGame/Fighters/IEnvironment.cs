namespace Fighters;

public interface IEnvironment
{
    void WriteLine( string message );
    void Write( string message );

    string? ReadLine();
}
