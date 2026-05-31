namespace Domain.Exceptions;

public class GuestCountOutOfRangeException : DomainException
{
    public int MinAllowed { get; }
    public int MaxAllowed { get; }
    public int ActualCount { get; }

    public GuestCountOutOfRangeException( int min, int max, int actual )
        : base( $"Guest count must be between {min} and {max}. Provided: {actual}." )
    {
        MinAllowed = min;
        MaxAllowed = max;
        ActualCount = actual;
    }
}
