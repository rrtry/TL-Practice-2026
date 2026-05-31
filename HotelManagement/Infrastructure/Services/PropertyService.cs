using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;

    public PropertyService( IPropertyRepository propertyRepository, IRoomTypeRepository roomTypeRepository, IReservationRepository reservationRepository )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<IEnumerable<Property>> GetAllPropertiesAsync() =>
        await _propertyRepository.GetAllAsync();

    public async Task<Property?> GetPropertyByIdAsync( Guid id ) =>
        await _propertyRepository.GetByIdAsync( id );

    public async Task<Property> CreatePropertyAsync( Property property )
    {
        await _propertyRepository.AddAsync( property );
        return property;
    }

    public async Task UpdatePropertyAsync( Property property )
    {
        var existing = await _propertyRepository.GetByIdAsync( property.Id );
        if ( existing == null )
        {
            throw new PropertyNotFoundException( property.Id );
        }

        await _propertyRepository.UpdateAsync( property );
    }

    public async Task DeletePropertyAsync( Guid id )
    {
        var exists = await _propertyRepository.ExistsAsync( id );
        if ( !exists )
        {
            throw new PropertyNotFoundException( id );
        }

        var roomTypes = await _roomTypeRepository.GetByPropertyIdAsync( id );
        // Нельзя удалить недвижимость с существующими номерами
        if ( roomTypes.Any() )
        {
            throw new InvalidOperationException( "Cannot delete property with existing room types." );
        }

        // Нельзя удалить недвижимость с существующими бронями
        var reservations = await _reservationRepository.GetFilteredAsync( id, null, null, null );
        if ( reservations.Any() )
        {
            throw new InvalidOperationException( "Cannot delete property with existing reservations." );
        }

        await _propertyRepository.DeleteAsync( id );
    }
}