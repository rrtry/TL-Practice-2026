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

    public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
    {
        return await _propertyRepository.GetAllAsync();
    }

    public async Task<Property> GetPropertyByIdAsync( Guid id )
    {
        var property = await _propertyRepository.GetByIdAsync( id );
        if ( property == null )
        {
            throw new PropertyNotFoundException( id );
        }

        return property;
    }

    public async Task<Property> CreatePropertyAsync( Property property )
    {
        await _propertyRepository.AddAsync( property );
        await _unitOfWork.SaveChangesAsync();
        return property;
    }

    public async Task UpdatePropertyAsync( Property property )
    {
        var existing = await _propertyRepository.GetByIdAsyncForUpdate( property.Id );

        if ( existing == null )
        {
            throw new PropertyNotFoundException( property.Id );
        }

        await _propertyRepository.UpdateAsync( property );
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePropertyAsync( Guid id )
    {
        var property = await _propertyRepository.GetByIdAsyncForUpdate( id );

        if ( property == null )
        {
            throw new PropertyNotFoundException( id );
        }

        if ( await _roomTypeRepository.HasRoomTypesForPropertyAsync( id ) )
        {
            throw new InvalidOperationException( "Cannot delete property with existing room types." );
        }

        if ( await _reservationRepository.HasReservationsAsync( id ) )
        {
            throw new InvalidOperationException( "Cannot delete property with existing reservations." );
        }

        _propertyRepository.Delete( property );
        await _unitOfWork.SaveChangesAsync();
    }
}