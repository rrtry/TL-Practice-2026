using Domain.Entities;
using HotelManagement.WebApi.Dto;

namespace HotelManagement.WebApi.Mappers;

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
        if ( request.Name != null )
        {
            existing.Name = request.Name;
        }

        if ( request.Country != null )
        {
            existing.Country = request.Country;
        }

        if ( request.City != null )
        {
            existing.City = request.City;
        }

        if ( request.Address != null )
        {
            existing.Address = request.Address;
        }

        if ( request.Latitude != null )
        {
            existing.Latitude = request.Latitude!.Value;
        }

        if ( request.Longitude != null )
        {
            existing.Longitude = request.Longitude!.Value;
        }
    }
}
