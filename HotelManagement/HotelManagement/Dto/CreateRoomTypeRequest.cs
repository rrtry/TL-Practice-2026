using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class CreateRoomTypeRequest
{
    [Required]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "RoomType name length must be in range [1:255]" )]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range( 0, double.MaxValue, ErrorMessage = "RoomType dailyPrice must be non-negative" )]
    public decimal DailyPrice { get; set; }

    [Required]
    [StringLength( 3, MinimumLength = 3, ErrorMessage = "RoomType currency length must be 3" )]
    public string Currency { get; set; } = "RUB";

    [Required]
    [Range( 1, int.MaxValue, ErrorMessage = "RoomType minPersonCount must be >= 1" )]
    public int MinPersonCount { get; set; }

    [Required]
    [Range( 1, int.MaxValue, ErrorMessage = "RoomType maxPersonCount must be >= 1" )]
    public int MaxPersonCount { get; set; }

    [Required]
    [Range( 1, int.MaxValue, ErrorMessage = "RoomType availableRoomsCount must be >= 1" )]
    public int AvailableRoomsCount { get; set; }

    public List<string> Services { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
}
