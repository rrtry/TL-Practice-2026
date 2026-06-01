using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class UpdateRoomTypeRequest
{
    [Required( ErrorMessage = "Name is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "RoomType name length must be in range [1:255]" )]
    public string Name { get; set; } = null!;

    [Required( ErrorMessage = "DailyPrice is required" )]
    [Range( 0, double.MaxValue, ErrorMessage = "RoomType dailyPrice must be non-negative" )]
    public decimal? DailyPrice { get; set; }

    [Required( ErrorMessage = "Currency is required" )]
    [StringLength( 3, MinimumLength = 3, ErrorMessage = "RoomType currency length must be 3" )]
    public string Currency { get; set; } = null!;

    [Required( ErrorMessage = "MinPersonCount is required" )]
    [Range( 1, int.MaxValue, ErrorMessage = "RoomType minPersonCount must be >= 1" )]
    public int? MinPersonCount { get; set; }

    [Required( ErrorMessage = "MaxPersonCount is required" )]
    [Range( 1, int.MaxValue, ErrorMessage = "RoomType maxPersonCount must be >= 1" )]
    public int? MaxPersonCount { get; set; }

    [Required( ErrorMessage = "AvailableRoomsCount is required" )]
    [Range( 1, int.MaxValue, ErrorMessage = "RoomType availableRoomsCount must be >= 1" )]
    public int? AvailableRoomsCount { get; set; }

    public List<string> Services { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
}
