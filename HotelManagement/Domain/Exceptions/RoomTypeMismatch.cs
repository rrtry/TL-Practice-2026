namespace Domain.Exceptions;

public class RoomTypeMismatchException : DomainException
{
    public Guid RoomTypeId { get; }
    public Guid ExpectedPropertyId { get; }
    public Guid ActualPropertyId { get; }

    public RoomTypeMismatchException( Guid roomTypeId, Guid expectedPropertyId, Guid actualPropertyId )
        : base( $"Room type '{roomTypeId}' does not belong to property '{expectedPropertyId}'." )
    {
        RoomTypeId = roomTypeId;
        ExpectedPropertyId = expectedPropertyId;
        ActualPropertyId = actualPropertyId;
    }
}
