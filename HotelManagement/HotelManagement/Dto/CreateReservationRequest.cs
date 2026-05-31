using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class CreateReservationRequest
{
    [Required]
    public Guid PropertyId { get; set; }

    [Required]
    public Guid RoomTypeId { get; set; }

    [Required]
    public DateOnly ArrivalDate { get; set; }

    [Required]
    public DateOnly DepartureDate { get; set; }

    [Required]
    public TimeOnly ArrivalTime { get; set; }

    [Required]
    public TimeOnly DepartureTime { get; set; }

    [Required]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "GuestName length must be in range [1:255]" )]
    public string GuestName { get; set; } = string.Empty;

    [Required]
    [Phone]
    public string GuestPhoneNumber { get; set; } = string.Empty;

    [Required]
    [Range( 1, 10, ErrorMessage = "GuestCount must be in range [1:10]" )]
    public int GuestCount { get; set; }
}
