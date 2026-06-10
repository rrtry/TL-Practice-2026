using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId );
    Task<RoomType?> GetByIdAsyncForUpdate( Guid id );
    Task<RoomType?> GetByIdAsync( Guid id );
    Task AddAsync( RoomType roomType );
    Task UpdateAsync( RoomType roomType );
    Task DeleteByIdAsync( Guid id );
    void Delete( RoomType roomType );
    Task<bool> ExistsAsync( Guid id );
}
