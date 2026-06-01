using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class CreatePropertyRequest
{
    [Required( ErrorMessage = "Property name is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property name length should be in range: [1;255]" )]
    public string Name { get; set; } = null!;

    [Required( ErrorMessage = "Property country is required" )]
    [StringLength( 3, MinimumLength = 3, ErrorMessage = "Property country length should be 3" )]
    public string Country { get; set; } = null!;

    [Required( ErrorMessage = "Property city is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property city length should be in range: [1;255]" )]
    public string City { get; set; } = null!;

    [Required( ErrorMessage = "Property Address is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property address length should be in range: [1;255]" )]
    public string Address { get; set; } = null!;

    [Required( ErrorMessage = "Property latitude is required" )]
    [Range( -90.0, 90.0, ErrorMessage = "Latitude valid range is [-90;90]" )]
    public double? Latitude { get; set; }

    [Required( ErrorMessage = "Property longitude is required" )]
    [Range( -180.0, 180.0, ErrorMessage = "Longitude valid range is [-180;180]" )]
    public double? Longitude { get; set; }
}
