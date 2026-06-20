namespace Domain.Exceptions;

public class InvalidArrivalDateException : DomainException
{
    public DateOnly ArrivalDate { get; }
    public DateOnly Today { get; }

    public InvalidArrivalDateException( DateOnly arrival, DateOnly today )
        : base( $"Arrival ({arrival:d}) date must be after or equal to today's date ({today:d})." )
    {
        ArrivalDate = arrival;
        Today = today;
    }
}
