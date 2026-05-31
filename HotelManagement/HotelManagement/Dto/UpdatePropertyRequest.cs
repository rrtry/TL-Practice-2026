using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class UpdatePropertyRequest
{
    [Required]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property name length should be in range: [1;255]" )]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength( 3, MinimumLength = 3, ErrorMessage = "Property country length should be 3" )]
    public string Country { get; set; } = string.Empty;

    [Required]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property city length should be in range: [1;255]" )]
    public string City { get; set; } = string.Empty;

    [Required]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Property address length should be in range: [1;255]" )]
    public string Address { get; set; } = string.Empty;

    [Required]
    [Range( -90.0, 90.0, ErrorMessage = "Latitude valid range is [-90;90]" )]
    public double Latitude { get; set; }

    [Required]
    [Range( -180.0, 180.0, ErrorMessage = "Longitude valid range is [-180;180]" )]
    public double Longitude { get; set; }
}
