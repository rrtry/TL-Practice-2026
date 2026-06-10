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
            DailyPrice = request.DailyPrice!.Value,
            Currency = request.Currency,
            MinPersonCount = request.MinPersonCount!.Value,
            MaxPersonCount = request.MaxPersonCount!.Value,
            AvailableRoomsCount = request.AvailableRoomsCount!.Value,
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
        if ( request.Name != null )
        {
            existing.Name = request.Name;
        }

        if ( request.DailyPrice != null )
        {
            existing.DailyPrice = request.DailyPrice!.Value;
        }

        if ( request.Currency != null )
        {
            existing.Currency = request.Currency;
        }

        if ( request.MinPersonCount != null )
        {
            existing.MinPersonCount = request.MinPersonCount!.Value;
        }

        if ( request.MaxPersonCount != null )
        {
            existing.MaxPersonCount = request.MaxPersonCount!.Value;
        }

        if ( request.AvailableRoomsCount != null )
        {
            existing.AvailableRoomsCount = request.AvailableRoomsCount!.Value;
        }

        if ( request.Services != null )
        {
            existing.Services = request.Services;
        }

        if ( request.Amenities != null )
        {
            existing.Amenities = request.Amenities;
        }
    }
}
