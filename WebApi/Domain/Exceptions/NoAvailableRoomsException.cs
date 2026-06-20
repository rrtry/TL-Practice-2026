namespace Domain.Exceptions;

public class NoAvailableRoomsException : DomainException
{
    public Guid RoomTypeId { get; }
    public DateOnly ArrivalDate { get; }
    public DateOnly DepartureDate { get; }

    public NoAvailableRoomsException( Guid roomTypeId, DateOnly arrival, DateOnly departure )
        : base( $"No available rooms of type '{roomTypeId}' for period {arrival:d} - {departure:d}." )
    {
        RoomTypeId = roomTypeId;
        ArrivalDate = arrival;
        DepartureDate = departure;
    }
}
