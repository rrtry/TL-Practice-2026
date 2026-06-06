using Domain.Entities;

namespace Domain.Services;

public interface IRoomTypeService
{
    Task<IEnumerable<RoomType>> GetRoomTypesByPropertyIdAsync( Guid propertyId );
    Task<RoomType?> GetRoomTypeByIdAsync( Guid id );
    Task<RoomType> CreateRoomTypeAsync( Guid propertyId, RoomType roomType );
    Task UpdateRoomTypeAsync( RoomType roomType );
    Task DeleteRoomTypeAsync( Guid id );
}
