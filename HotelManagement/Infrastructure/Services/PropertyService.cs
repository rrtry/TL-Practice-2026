using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Filters;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IRoomTypeRepository roomTypeRepository,
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork )
    {
        _propertyRepository = propertyRepository;
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Property>> GetAllPropertiesAsync() =>
        await _propertyRepository.GetAllAsync();

    public async Task<Property?> GetPropertyByIdAsync( Guid id ) =>
        await _propertyRepository.GetByIdAsync( id );

    public async Task<Property> CreatePropertyAsync( Property property )
    {
        await _propertyRepository.AddAsync( property );
        await _unitOfWork.SaveChangesAsync();
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
        await _unitOfWork.SaveChangesAsync();
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

        var filter = new ReservationFilter();
        filter.PropertyId = id;

        // Нельзя удалить недвижимость с существующими бронями
        var reservations = await _reservationRepository.GetFilteredAsync( filter );
        if ( reservations.Any() )
        {
            throw new InvalidOperationException( "Cannot delete property with existing reservations." );
        }

        await _propertyRepository.DeleteAsync( id );
        await _unitOfWork.SaveChangesAsync();
    }
}