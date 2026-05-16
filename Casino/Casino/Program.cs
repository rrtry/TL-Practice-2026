using Casino;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

const int WIN_RANGE_MIN = 18;
const int WIN_RANGE_MAX = 20;

const int RAND_RANGE_MIN = 1;
const int RAND_RANGE_MAX = 21;

IReadOnlyList<string> menuOptions = [
    "0. Make Deposit",
    "1. Show Balance",
    "2. Play",
    "3. Exit"
];

decimal balance = 0;
bool isGameFinished = false;

PrintHeader();

while ( !isGameFinished )
{
    PrintMenu();
    OptionHandleResult result = HandleOptions();

    Console.WriteLine( result );
    Console.WriteLine();
}

void PrintHeader()
{
    Console.WriteLine( "Casino\n" );
}

void PrintMenu()
{
    foreach ( string option in menuOptions )
    {
        Console.WriteLine( option );
    }
}

OptionHandleResult HandleOptions()
{
    string input = Console.ReadLine() ?? "";
    if ( int.TryParse( input, out int result ) )
    {
        MenuOption option = ( MenuOption )result;
        switch ( option )
        {
            case MenuOption.MakeDeposit:
                return MakeDeposit();

            case MenuOption.ShowBalance:
                return ShowBalance();

            case MenuOption.Play:
                return Play();

            case MenuOption.Exit:
                return Exit();

            default:
                return OptionHandleResult.InvalidOption;
        }
    }

    return OptionHandleResult.InvalidOption;
}

OptionHandleResult MakeDeposit()
{
    Console.Write( "Enter amount: " );
    string depositString = Console.ReadLine()!;

    if ( !ParsePositiveDecimal( depositString, out decimal deposit ) )
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

bool MakeBet( out decimal bet )
{
    Console.Write( "Your bet is: " );
    string betStr = Console.ReadLine()!;

    if ( !ParsePositiveDecimal( betStr, out bet ) )
    {
        return false;
    }

    if ( bet > balance )
    {
        return false;
    }

    return true;
}

OptionHandleResult Play()
{
    decimal bet;
    if ( !MakeBet( out bet ) )
    {
        return OptionHandleResult.InvalidBet;
    }

    int seed = Random.Shared.Next( RAND_RANGE_MIN, RAND_RANGE_MAX );
    Console.WriteLine( $"Rolling the dice... Your number is {seed}" );

    if ( seed >= WIN_RANGE_MIN && seed <= WIN_RANGE_MAX )
    {
        decimal winAmount = CalculateWinAmount( bet, seed );
        balance += winAmount;
        Console.WriteLine( $"You won {winAmount}! Your balance is {balance}, your bet was {bet}" );
    }
    else
    {
        balance -= bet;
        Console.WriteLine( $"You lost {bet}" );
    }

    return OptionHandleResult.Success;
}

decimal CalculateWinAmount( decimal bet, int seed )
{
    const int multiplicator = 25;
    const int normalizer = 17;
    decimal winPrecent = multiplicator * ( seed % normalizer );

    if ( winPrecent <= 0 )
    {
        return 0;
    }

    return bet * ( 1 + winPrecent / 100 );
}

static bool ParsePositiveDecimal( string input, out decimal result )
{
    if ( decimal.TryParse( input, out result ) && result > 0 )
    {
        return true;
    }

    result = 0;

    return false;
}

OptionHandleResult Exit()
{
    isGameFinished = true;
    return OptionHandleResult.Success;
}