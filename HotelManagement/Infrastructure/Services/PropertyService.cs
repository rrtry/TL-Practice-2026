using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PropertyService(
        IPropertyRepository propertyRepository,
        IReservationRepository reservationRepository,
        IUnitOfWork unitOfWork )
    {
        _propertyRepository = propertyRepository;
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

    public async Task<Property> GetPropertyByIdForUpdateAsync( Guid id )
    {
        var property = await _propertyRepository.GetByIdForUpdateAsync( id );

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
        await _propertyRepository.UpdateAsync( property );
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePropertyAsync( Guid id )
    {
        var property = await _propertyRepository.GetByIdForUpdateAsync( id );

        if ( property == null )
        {
            throw new PropertyNotFoundException( id );
        }

        if ( await _reservationRepository.HasReservationsAsync( id ) )
        {
            throw new PropertyHasExistingReservationsException( "Cannot delete property with existing reservations." );
        }

        _propertyRepository.Delete( property );
        await _unitOfWork.SaveChangesAsync();
    }
}