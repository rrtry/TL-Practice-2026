const string PROMPT_PRODUCT_TITLE = "Введите название товара: ";
const string PROMPT_PRODUCT_QUANTITY = "Введите количество товара: ";
const string PROMPT_USER_NAME = "Введите ваше имя: ";
const string PROMPT_USER_ADDRESS = "Введите адрес доставки: ";
const string PROMPT_CONTINUE_SHOPPING = "Продолжить покупки? (y/n): ";

const string MESSAGE_ERROR_EMPTY_STRING = "Ошибка: значение не может быть пустым. Попробуйте снова.";
const string MESSAGE_ERROR_INVALID_QUANTITY = "Ошибка: неверное число товаров. Попробуйте снова.";
const string MESSAGE_ERROR_EMPTY_STDIN = "Ошибка: поток ввода пустой";
const string MESSAGE_ERROR_PROMPT_ANSWER = "Ошибка: Ответьте да/нет";

const string MESSAGE_ORDER_CANCELLED = "Заказ отменён.";
const string DATE_FORMAT = "dd.MM.yyyy";

string product;
int quantity;

string userName;
string address;

SetConsoleEncoding();
Run();

void Run()
{
    bool run = true;
    while ( run )
    {
        ReadProductInfo();
        ReadUserInfo();
        ProcessOrder();

        if ( !PromptConfirmation( PROMPT_CONTINUE_SHOPPING ) )
        {
            run = false;
        }
    }

    Console.WriteLine( "Пока." );
}

void SetConsoleEncoding()
{
    Console.InputEncoding = System.Text.Encoding.UTF8;
    Console.OutputEncoding = System.Text.Encoding.UTF8;
}

void ProcessOrder()
{
    string message = $"Здравствуйте, {userName}, вы заказали {quantity} {product} на адрес {address}, все верно? (y/n): ";
    bool isConfirmed = PromptConfirmation( message );

    if ( isConfirmed )
    {
        DateTime arrivalDate = DateTime.Today.AddDays( 3 );
        PrintSuccess( userName, product, quantity, address, arrivalDate );
    }
    else
    {
        PrintCancelled();
    }
}

void ReadProductInfo()
{
    product = ReadString( PROMPT_PRODUCT_TITLE );
    quantity = ReadQuantity( PROMPT_PRODUCT_QUANTITY );
}

void ReadUserInfo()
{
    userName = ReadString( PROMPT_USER_NAME );
    address = ReadString( PROMPT_USER_ADDRESS );
}

string ReadLineTrimmed()
{
    return Console.ReadLine()?.Trim() ?? throw new EndOfStreamException( MESSAGE_ERROR_EMPTY_STDIN );
}

string ReadString( string prompt )
{
    string result;
    do
    {
        Console.Write( prompt );
        result = ReadLineTrimmed();

        if ( string.IsNullOrWhiteSpace( result ) )
        {
            Console.WriteLine( MESSAGE_ERROR_EMPTY_STRING );
        }
        else
        {
            break;
        }

    } while ( true );
    return result;
}

int ReadQuantity( string prompt )
{
    int result;
    do
    {
        Console.Write( prompt );
        string input = ReadLineTrimmed();
        if ( !int.TryParse( input, out result ) || result <= 0 )
        {
            Console.WriteLine( MESSAGE_ERROR_INVALID_QUANTITY );
        }
        else
        {
            break;
        }

    } while ( true );
    return result;
}

bool PromptConfirmation( string message )
{
    do
    {
        Console.Write( message );
        string answer = ReadLineTrimmed().ToLowerInvariant();

        switch ( answer )
        {
            case "y":
            case "yes":
            case "д":
            case "да":
                return true;

            case "n":
            case "no":
            case "н":
            case "нет":
                return false;

            default:
                Console.WriteLine( MESSAGE_ERROR_PROMPT_ANSWER );
                break;
        }
    } while ( true );
}

void PrintSuccess( string name, string product, int count, string address, DateTime arrivalDate )
{
    string formattedDate = arrivalDate.ToString( DATE_FORMAT );
    Console.WriteLine( $"{name}! Ваш заказ {product} в количестве {count} оформлен! Ожидайте доставку по адресу {address} {formattedDate}" );
}

void PrintCancelled()
{
    Console.WriteLine( MESSAGE_ORDER_CANCELLED );
}