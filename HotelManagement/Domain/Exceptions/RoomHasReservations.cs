namespace Domain.Exceptions;

public class RoomTypeHasReservationsException : DomainException
{
    public Guid RoomTypeId { get; }

    public RoomTypeHasReservationsException( Guid roomTypeId )
        : base( $"Cannot delete room type '{roomTypeId}' because it has existing reservations." )
    {
        RoomTypeId = roomTypeId;
    }
}
