using CarFactory.Factory;
using CarFactory.Models;
using CarFactory.Models.Interfaces;

namespace CarFactory.UI;

public class ConsoleCarCreator : ICarCreator
{
    public Car CreateCar()
    {
        Console.WriteLine( "How would you like to configure your car?" );
        Console.WriteLine( "  1. Choose a preset configuration" );
        Console.WriteLine( "  2. Build manually from scratch" );

        int choice = ReadChoiceNumber( 1, 2 );
        CarBuilder builder = choice == 1 ? ChoosePreset() : BuildManually();

        return builder.Build();
    }

    private CarBuilder ChoosePreset()
    {
        Console.WriteLine( "\nAvailable presets:" );

        for ( int i = 0; i < CarCatalog.Presets.Count; i++ )
        {
            var preset = CarCatalog.Presets[ i ];
            Console.WriteLine( $"  {i + 1}. {preset.Name}" );
        }

        int choice = ReadChoiceNumber( 1, CarCatalog.Presets.Count );
        var selectedPreset = CarCatalog.Presets[ choice - 1 ];
        return selectedPreset.Configuration;
    }

    private CarBuilder BuildManually()
    {
        var builder = new CarBuilder();
        Console.WriteLine( "\nManual configuration" );

        builder.SetBrand( ChooseBrand() );
        builder.SetColor( ChooseColor() );
        builder.SetBody( ChooseBody() );
        builder.SetEngine( ChooseEngine() );
        builder.SetTransmission( ChooseTransmission() );
        builder.SetWheels( ChooseWheels() );

        return builder;
    }

    private IBrand ChooseBrand() => Choose( "brand", CarCatalog.Brands );
    private IColor ChooseColor() => Choose( "color", CarCatalog.Colors );
    private IBody ChooseBody() => Choose( "body type", CarCatalog.Bodies );
    private IEngine ChooseEngine() => Choose( "engine", CarCatalog.Engines );
    private ITransmission ChooseTransmission() => Choose( "transmission", CarCatalog.Transmissions );
    private IWheels ChooseWheels() => Choose( "wheel type", CarCatalog.Wheels );

    private static T Choose<T>( string partName, IReadOnlyList<T> options ) where T : class, IDisplay
    {
        Console.WriteLine( $"\nSelect {partName}:" );
        for ( int i = 0; i < options.Count; i++ )
        {
            Console.WriteLine( $"  {i + 1}. {options[ i ].Name}" );
        }

        int choice = ReadChoiceNumber( 1, options.Count );
        return options[ choice - 1 ];
    }

    public bool AskToContinue()
    {
        Console.Write( "\nCreate another car? (y/n): " );
        return Console.ReadLine()?.Trim().ToLower() == "y";
    }

    private static int ReadChoiceNumber( int min, int max )
    {
        while ( true )
        {
            Console.Write( $"Your choice [{min}-{max}]: " );
            if ( int.TryParse( Console.ReadLine(), out int value ) && value >= min && value <= max )
            {
                return value;
            }

            Console.WriteLine( $"Invalid input. Please enter a number from {min} to {max}." );
        }
    }
}