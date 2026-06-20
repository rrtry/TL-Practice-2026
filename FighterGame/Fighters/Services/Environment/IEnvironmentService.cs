namespace Fighters.Services.Environment;

public interface IEnvironmentService
{
    void WriteLine( string message );
    void Write( string message );
    string? ReadLine();
}
