namespace Domain.Exceptions;

public class InvalidArrivalTimeException : DomainException
{
    public TimeOnly ArrivalTime { get; }
    public TimeOnly Now { get; }

    public InvalidArrivalTimeException( TimeOnly arrival, TimeOnly now )
        : base( $"Arrival ({arrival}) time must be before the current time ({now})." )
    {
        ArrivalTime = arrival;
        Now = now;
    }
}