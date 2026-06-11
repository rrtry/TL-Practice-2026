using System.ComponentModel.DataAnnotations;
using HotelManagement.WebApi.Validators;

namespace HotelManagement.WebApi.Dto;

public class UpdateRoomTypeRequest
{
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "RoomType Name length must be in range [1:255]" )]
    public string? Name { get; set; } = null;

    [Range( 0, double.MaxValue, ErrorMessage = "RoomType DailyPrice must be non-negative" )]
    public decimal? DailyPrice { get; set; } = null;

    [CurrencyCode( ErrorMessage = "RoomType Currency must be a valid ISO 4217 currency code." )]
    public string? Currency { get; set; } = null;

    [Range( 1, int.MaxValue, ErrorMessage = "RoomType MinPersonCount must be >= 1" )]
    public int? MinPersonCount { get; set; } = null;

    [Range( 1, int.MaxValue, ErrorMessage = "RoomType MaxPersonCount must be >= 1" )]
    public int? MaxPersonCount { get; set; } = null;

    [Range( 1, int.MaxValue, ErrorMessage = "RoomType AvailableRoomsCount must be >= 1" )]
    public int? AvailableRoomsCount { get; set; } = null;

    public List<string>? Services { get; set; } = null;
    public List<string>? Amenities { get; set; } = null;
}