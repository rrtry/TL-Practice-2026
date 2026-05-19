using CarFactory.Models;
using CarFactory.UI;

Console.Clear();
Console.WriteLine( "Car Factory" );

ICarCreator carCreator = new ConsoleCarCreator();

while ( true )
{
    Car car = carCreator.CreateCar();
    Console.WriteLine( car );

    if ( !carCreator.AskToContinue() )
    {
        break;
    }
}

Console.WriteLine( "\nBye. " );