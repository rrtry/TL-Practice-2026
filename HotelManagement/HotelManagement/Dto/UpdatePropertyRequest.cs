using System.ComponentModel.DataAnnotations;
using HotelManagement.WebApi.Validators;

namespace HotelManagement.WebApi.Dto;

public class UpdatePropertyRequest
{
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property Name length should be in range: [1;255]" )]
    public string? Name { get; set; } = null;

    [CountryCode( ErrorMessage = "Property Country must be a valid ISO 3166-1 country code" )]
    public string? Country { get; set; } = null;

    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property City length should be in range: [1;255]" )]
    public string? City { get; set; } = null;

    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property Address length should be in range: [1;255]" )]
    public string? Address { get; set; } = null;

    [Range( -90.0, 90.0, ErrorMessage = "Property Latitude valid range is [-90;90]" )]
    public double? Latitude { get; set; } = null;

    [Range( -180.0, 180.0, ErrorMessage = "Property Longitude valid range is [-180;180]" )]
    public double? Longitude { get; set; } = null;
}
