namespace Domain.Exceptions;

public class PropertyNotFoundException : DomainException
{
    public Guid PropertyId { get; }

    public PropertyNotFoundException( Guid propertyId )
        : base( $"Property with id '{propertyId}' was not found." )
    {
        PropertyId = propertyId;
    }
}
