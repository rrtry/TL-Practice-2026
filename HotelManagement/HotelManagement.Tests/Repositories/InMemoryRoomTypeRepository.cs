using System.Collections.Concurrent;

using Domain.Entities;
using Domain.Repositories;

namespace HotelManagement.Tests.Repositories;

public class InMemoryRoomTypeRepository : IRoomTypeRepository
{
    private readonly ConcurrentDictionary<Guid, RoomType> _roomTypes = new();

    public Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId )
    {
        return Task.FromResult( _roomTypes.Values.Where( rt => rt.PropertyId == propertyId ) );
    }

    public Task<RoomType?> GetByIdAsync( Guid id )
    {
        return GetByIdAsyncForUpdate( id );
    }

    public Task<RoomType?> GetByIdAsyncForUpdate( Guid id )
    {
        return Task.FromResult( _roomTypes.TryGetValue( id, out var rt ) ? rt : null );
    }

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

    public Task DeleteByIdAsync( Guid id )
    {
        _roomTypes.TryRemove( id, out _ );
        return Task.CompletedTask;
    }

    public void Delete( RoomType roomType )
    {
        _roomTypes.TryRemove( roomType.Id, out _ );
    }

    public Task<bool> ExistsAsync( Guid id )
    {
        return Task.FromResult( _roomTypes.ContainsKey( id ) );
    }

    public Task<bool> HasRoomTypesForPropertyAsync( Guid propertyId )
    {
        var exists = _roomTypes.Values.Any( rt => rt.PropertyId == propertyId );
        return Task.FromResult( exists );
    }
}