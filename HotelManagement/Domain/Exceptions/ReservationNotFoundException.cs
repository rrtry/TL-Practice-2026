namespace Domain.Exceptions;

public class ReservationNotFoundException : DomainException
{
    public Guid ReservationId { get; }

    public ReservationNotFoundException( Guid reservationId )
        : base( $"Reservation with id '{reservationId}' was not found." )
    {
        ReservationId = reservationId;
    }
}
