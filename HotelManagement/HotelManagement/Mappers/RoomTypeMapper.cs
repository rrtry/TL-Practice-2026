using Domain.Entities;
using HotelManagement.Dto;

namespace HotelManagement.Mappers;

public static class RoomTypeMapper
{
    public static RoomType MapCreateRequestToEntity( CreateRoomTypeRequest request )
    {
        return new RoomType
        {
            Name = request.Name,
            DailyPrice = request.DailyPrice,
            Currency = request.Currency,
            MinPersonCount = request.MinPersonCount,
            MaxPersonCount = request.MaxPersonCount,
            AvailableRoomsCount = request.AvailableRoomsCount,
            Services = request.Services,
            Amenities = request.Amenities
        };
    }

    public static RoomTypeResponse MapEntityToResponse( RoomType entity )
    {
        return new RoomTypeResponse
        {
            Id = entity.Id,
            PropertyId = entity.PropertyId,
            Name = entity.Name,
            DailyPrice = entity.DailyPrice,
            Currency = entity.Currency,
            MinPersonCount = entity.MinPersonCount,
            MaxPersonCount = entity.MaxPersonCount,
            AvailableRoomsCount = entity.AvailableRoomsCount,
            Services = entity.Services,
            Amenities = entity.Amenities
        };
    }

    public static void Update( RoomType existing, UpdateRoomTypeRequest request )
    {
        existing.Name = request.Name;
        existing.DailyPrice = request.DailyPrice;
        existing.Currency = request.Currency;
        existing.MinPersonCount = request.MinPersonCount;
        existing.MaxPersonCount = request.MaxPersonCount;
        existing.AvailableRoomsCount = request.AvailableRoomsCount;
        existing.Services = request.Services;
        existing.Amenities = request.Amenities;
    }
}
