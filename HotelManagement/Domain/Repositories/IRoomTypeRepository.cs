using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId );
    Task<RoomType?> GetByIdForUpdateAsync( Guid id );
    Task<RoomType?> GetByIdAsync( Guid id );
    Task AddAsync( RoomType roomType );
    Task UpdateAsync( RoomType roomType );
    void Delete( RoomType roomType );
    Task<bool> ExistsAsync( Guid id );
    Task<bool> HasRoomTypesForPropertyAsync( Guid propertyId );
}
