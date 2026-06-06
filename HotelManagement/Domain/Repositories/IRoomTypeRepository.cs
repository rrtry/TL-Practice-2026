using Domain.Entities;

namespace Domain.Repositories;

public interface IRoomTypeRepository
{
    Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId );
    Task<RoomType?> GetByIdAsync( Guid id );
    Task AddAsync( RoomType roomType );
    Task UpdateAsync( RoomType roomType );
    Task DeleteAsync( Guid id );
    Task<bool> ExistsAsync( Guid id );
    Task<bool> HasReservationsAsync( Guid roomTypeId );
}
