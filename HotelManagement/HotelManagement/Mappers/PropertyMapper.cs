using Domain.Entities;
using HotelManagement.Dto;

namespace HotelManagement.Mappers;

public static class PropertyMapper
{
    public static Property MapCreateRequestToEntity( CreatePropertyRequest request )
    {
        return new Property
        {
            Name = request.Name,
            Country = request.Country,
            City = request.City,
            Address = request.Address,
            Latitude = request.Latitude!.Value,
            Longitude = request.Longitude!.Value
        };
    }

    public static PropertyResponse MapEntityToResponse( Property created )
    {
        return new PropertyResponse
        {
            Id = created.Id,
            Name = created.Name,
            Country = created.Country,
            City = created.City,
            Address = created.Address,
            Latitude = created.Latitude,
            Longitude = created.Longitude
        };
    }

    public static void Update( Property existing, UpdatePropertyRequest request )
    {
        existing.Name = request.Name;
        existing.Country = request.Country;
        existing.City = request.City;
        existing.Address = request.Address;
        existing.Latitude = request.Latitude!.Value;
        existing.Longitude = request.Longitude!.Value;
    }
}
