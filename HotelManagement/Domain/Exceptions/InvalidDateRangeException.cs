namespace Domain.Exceptions;

public class InvalidDateRangeException : DomainException
{
    public DateOnly ArrivalDate { get; }
    public DateOnly DepartureDate { get; }

    public InvalidDateRangeException( DateOnly arrival, DateOnly departure )
        : base( $"Arrival date ({arrival:d}) must be before departure date ({departure:d})." )
    {
        ArrivalDate = arrival;
        DepartureDate = departure;
    }
}
