namespace Domain.Exceptions;

public class PropertyHasExistingReservationsException : DomainException
{
    public PropertyHasExistingReservationsException( string message ) : base( message ) { }
}