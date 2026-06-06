using System.Collections.Concurrent;

using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class InMemoryRoomTypeRepository : IRoomTypeRepository
{
    private readonly ConcurrentDictionary<Guid, RoomType> _roomTypes = new();

    public Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId ) =>
        Task.FromResult( _roomTypes.Values.Where( rt => rt.PropertyId == propertyId ) );

    public Task<RoomType?> GetByIdAsync( Guid id ) =>
        Task.FromResult( _roomTypes.TryGetValue( id, out var rt ) ? rt : null );

    public Task AddAsync( RoomType roomType )
    {
        roomType.Id = Guid.NewGuid();
        _roomTypes.TryAdd( roomType.Id, roomType );
        return Task.CompletedTask;
    }

    public Task UpdateAsync( RoomType roomType )
    {
        _roomTypes[ roomType.Id ] = roomType;
        return Task.CompletedTask;
    }

    public Task DeleteAsync( Guid id )
    {
        _roomTypes.TryRemove( id, out _ );
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync( Guid id ) =>
        Task.FromResult( _roomTypes.ContainsKey( id ) );

    public Task<bool> HasReservationsAsync( Guid roomTypeId )
    {
        throw new NotImplementedException( "Use service layer to check reservations." );
    }
}