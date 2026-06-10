using System.ComponentModel.DataAnnotations;

namespace HotelManagement.WebApi.Dto;

public class CreatePropertyRequest
{
    [Required( ErrorMessage = "Property Name is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property Name length should be in range: [1;255]" )]
    public string Name { get; set; } = null!;

    [Required( ErrorMessage = "Property Country is required" )]
    [StringLength( 3, MinimumLength = 3, ErrorMessage = "Property Country length should be 3" )]
    public string Country { get; set; } = null!;

    [Required( ErrorMessage = "Property City is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property City length should be in range: [1;255]" )]
    public string City { get; set; } = null!;

    [Required( ErrorMessage = "Property Address is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property Address length should be in range: [1;255]" )]
    public string Address { get; set; } = null!;

    [Required( ErrorMessage = "Property Latitude is required" )]
    [Range( -90.0, 90.0, ErrorMessage = "Property Latitude valid range is [-90;90]" )]
    public double? Latitude { get; set; }

    [Required( ErrorMessage = "Property Longitude is required" )]
    [Range( -180.0, 180.0, ErrorMessage = "Property Longitude valid range is [-180;180]" )]
    public double? Longitude { get; set; }
}
