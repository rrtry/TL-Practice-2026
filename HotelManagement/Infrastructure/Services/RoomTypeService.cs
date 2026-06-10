using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services;

namespace Infrastructure.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RoomTypeService(
        IRoomTypeRepository roomTypeRepository,
        IReservationRepository reservationRepository,
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork )
    {
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesByPropertyIdAsync( Guid propertyId )
    {
        // Недвижимость не найдена.
        if ( !await _propertyRepository.ExistsAsync( propertyId ) )
        {
            throw new PropertyNotFoundException( propertyId );
        }

        return await _roomTypeRepository.GetByPropertyIdAsync( propertyId );
    }

    public async Task<RoomType> GetRoomTypeByIdAsync( Guid id )
    {
        var roomType = await _roomTypeRepository.GetByIdAsync( id );

        if ( roomType == null )
        {
            throw new RoomTypeNotFoundException( id );
        }

        return roomType;
    }

    public async Task<RoomType> CreateRoomTypeAsync( Guid propertyId, RoomType roomType )
    {
        if ( !await _propertyRepository.ExistsAsync( propertyId ) )
        {
            throw new PropertyNotFoundException( propertyId );
        }

        roomType.PropertyId = propertyId;

        await _roomTypeRepository.AddAsync( roomType );
        await _unitOfWork.SaveChangesAsync();

        return roomType;
    }

    public async Task UpdateRoomTypeAsync( RoomType roomType )
    {
        await _roomTypeRepository.UpdateAsync( roomType );
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteRoomTypeAsync( Guid id )
    {
        var roomType = await _roomTypeRepository.GetByIdAsyncForUpdate( id );
        if ( roomType == null )
        {
            throw new RoomTypeNotFoundException( id );
        }

        // Проверка на пересекающиеся брони
        var anyReservations = await _reservationRepository.HasReservationsAsync( id );
        if ( anyReservations )
        {
            throw new RoomTypeHasReservationsException( id );
        }

        _roomTypeRepository.Delete( roomType );
        await _unitOfWork.SaveChangesAsync();
    }
}