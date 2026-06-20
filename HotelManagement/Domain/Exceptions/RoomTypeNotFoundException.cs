namespace Domain.Exceptions;

public class RoomTypeNotFoundException : DomainException
{
    public Guid RoomTypeId { get; }

    public RoomTypeNotFoundException( Guid roomTypeId )
        : base( $"Room type with id '{roomTypeId}' was not found." )
    {
        RoomTypeId = roomTypeId;
    }
}
