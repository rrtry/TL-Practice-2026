namespace Domain.Exceptions;

public class InvalidDateRangeException : DomainException
{
    public DateTime ArrivalDate { get; }
    public DateTime DepartureDate { get; }

    public InvalidDateRangeException( DateTime arrival, DateTime departure )
        : base( $"Arrival date ({arrival:d}) must be before departure date ({departure:d})." )
    {
        ArrivalDate = arrival;
        DepartureDate = departure;
    }
}
