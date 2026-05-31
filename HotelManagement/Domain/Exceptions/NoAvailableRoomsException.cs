namespace Domain.Exceptions;

public class NoAvailableRoomsException : DomainException
{
    public Guid RoomTypeId { get; }
    public DateTime ArrivalDate { get; }
    public DateTime DepartureDate { get; }

    public NoAvailableRoomsException( Guid roomTypeId, DateTime arrival, DateTime departure )
        : base( $"No available rooms of type '{roomTypeId}' for period {arrival:d} - {departure:d}." )
    {
        RoomTypeId = roomTypeId;
        ArrivalDate = arrival;
        DepartureDate = departure;
    }
}
