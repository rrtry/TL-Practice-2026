using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public class RoomTypeService : IRoomTypeService
{
    private readonly IRoomTypeRepository _roomTypeRepository;
    private readonly IReservationRepository _reservationRepository;
    private readonly IPropertyRepository _propertyRepository;

    public RoomTypeService( IRoomTypeRepository roomTypeRepository, IReservationRepository reservationRepository, IPropertyRepository propertyRepository )
    {
        _roomTypeRepository = roomTypeRepository;
        _reservationRepository = reservationRepository;
        _propertyRepository = propertyRepository;
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

    public async Task<RoomType?> GetRoomTypeByIdAsync( Guid id ) =>
        await _roomTypeRepository.GetByIdAsync( id );

    public async Task<RoomType> CreateRoomTypeAsync( Guid propertyId, RoomType roomType )
    {
        if ( !await _propertyRepository.ExistsAsync( propertyId ) )
        {
            throw new PropertyNotFoundException( propertyId );
        }

        roomType.PropertyId = propertyId;
        roomType.Id = Guid.NewGuid();
        await _roomTypeRepository.AddAsync( roomType );

        return roomType;
    }

    public async Task UpdateRoomTypeAsync( RoomType roomType )
    {
        var existing = await _roomTypeRepository.GetByIdAsync( roomType.Id );

        if ( existing == null )
        {
            throw new RoomTypeNotFoundException( roomType.Id );
        }

        roomType.PropertyId = existing.PropertyId;
        await _roomTypeRepository.UpdateAsync( roomType );
    }

    public async Task DeleteRoomTypeAsync( Guid id )
    {
        var exists = await _roomTypeRepository.ExistsAsync( id );
        if ( !exists )
        {
            throw new RoomTypeNotFoundException( id );
        }

        // Проверка на пересекающиеся брони
        var overlapping = await _reservationRepository.GetOverlappingReservationsCountAsync( id, DateOnly.MinValue, DateOnly.MaxValue );
        if ( overlapping > 0 )
        {
            throw new RoomTypeHasReservationsException( id );
        }

        await _roomTypeRepository.DeleteAsync( id );
    }
}