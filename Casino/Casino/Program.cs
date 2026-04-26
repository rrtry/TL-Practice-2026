Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintHeader();

decimal balance = 0;
bool isGameFinished = false;

while ( !isGameFinished )
{
    PrintMenu();

    string option = Console.ReadLine()!;
    OptionHandleResult result = HandleOptions( option );

    Console.WriteLine( result );
    Console.WriteLine();
}

void PrintHeader()
{
    const string header = "Casino\n";
    Console.WriteLine( header );
}

void PrintMenu()
{
    List<string> menuOptions = [
        "1. Make Deposit",
        "2. Show Balance",
        "3. Play",
        "4. Exit"
    ];

    foreach ( string option in menuOptions )
    {
        Console.WriteLine( option );
    }
}

OptionHandleResult HandleOptions( string option )
{
    switch ( option )
    {
        case "1":
            return MakeDeposit();
        case "2":
            return ShowBalance();
        case "3":
            return Play();
        case "4":
            return Exit();
        default:
            return OptionHandleResult.InvalidOption;
    }
}

OptionHandleResult MakeDeposit()
{
    Console.Write( "Enter amount: " );
    string depositString = Console.ReadLine()!;

    if ( !decimal.TryParse( depositString, out decimal deposit ) || deposit <= 0 )
    {
        return OptionHandleResult.InvalidDepositValue;
    }

    balance += deposit;
    return OptionHandleResult.Success;
}

OptionHandleResult ShowBalance()
{
    Console.WriteLine( $"Current balance is: {balance}" );
    return OptionHandleResult.Success;
}

OptionHandleResult Play()
{
    Console.Write( "Your bet is: " );
    string betStr = Console.ReadLine()!;

    if ( !decimal.TryParse( betStr, out decimal bet ) || bet <= 0 )
    {
        return OptionHandleResult.InvalidBet;
    }

    if ( bet > balance )
    {
        return OptionHandleResult.InvalidBet;
    }

    int seed = Random.Shared.Next( 1, 21 );
    if ( seed >= 18 && seed <= 20 )
    {
        decimal winAmount = CalculateWinAmount( bet, seed );
        balance += winAmount;
        Console.WriteLine( "You won!" );
    }
    else
    {
        balance -= bet;
        Console.WriteLine( "You lost" );
    }

    return OptionHandleResult.Success;
}

decimal CalculateWinAmount( decimal bet, int seed )
{
    const int multiplicator = 25;
    decimal winPrecent = multiplicator * ( seed % 17 );

    if ( winPrecent <= 0 )
    {
        return 0;
    }

    return bet * ( winPrecent / 100 );
}

OptionHandleResult Exit()
{
    isGameFinished = true;
    return OptionHandleResult.Success;
}

enum OptionHandleResult
{
    Success = 0,
    InvalidOption = 1,
    InvalidDepositValue = 2,
    InvalidBet = 3
}