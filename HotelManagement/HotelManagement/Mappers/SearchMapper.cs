using Domain.Filters;
using Domain.Models;
using HotelManagement.Dto;

namespace HotelManagement.Mappers;

public static class SearchMapper
{
    public static SearchFilter MapSearchRequestToFilter( SearchRequest request )
    {
        return new SearchFilter
        {
            City = request.City,
            ArrivalDate = request.ArrivalDate!.Value,
            DepartureDate = request.DepartureDate!.Value,
            Guests = request.Guests!.Value,
            MaxPrice = request.MaxPrice
        };
    }

    public static SearchResultResponse MapSearchResultToResponse( SearchResultItem item )
    {
        return new SearchResultResponse
        {
            PropertyId = item.PropertyId,
            PropertyName = item.PropertyName,
            City = item.City,
            RoomTypeId = item.RoomTypeId,
            RoomTypeName = item.RoomTypeName,
            DailyPrice = item.DailyPrice,
            Currency = item.Currency,
            TotalForStay = item.TotalForStay,
            AvailableRooms = item.AvailableRooms
        };
    }
}
