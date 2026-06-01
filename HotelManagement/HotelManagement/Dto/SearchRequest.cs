using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class SearchRequest
{
    [Required]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "City length must in range [1;255]" )]
    public string City { get; set; } = string.Empty;

    [Required( ErrorMessage = "ArrivalDate is required" )]
    public DateOnly ArrivalDate { get; set; }

    [Required( ErrorMessage = "DepartureDate is required" )]
    public DateOnly DepartureDate { get; set; }

    [Required( ErrorMessage = "Number of guests is required" )]
    [Range( 1, int.MaxValue, ErrorMessage = "Guests must be >= 1" )]
    public int Guests { get; set; }

    [Range( 0.0, double.MaxValue, ErrorMessage = "MaxPrice cannot be negative" )]
    public decimal? MaxPrice { get; set; }
}