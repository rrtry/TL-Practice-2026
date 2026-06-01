using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class CreateReservationRequest
{
    [Required( ErrorMessage = "PropertyId is required" )]
    public Guid? PropertyId { get; set; }

    [Required( ErrorMessage = "RoomTypeId is required" )]
    public Guid? RoomTypeId { get; set; }

    [Required( ErrorMessage = "ArrivalDate is required" )]
    public DateOnly? ArrivalDate { get; set; }

    [Required( ErrorMessage = "DepartureDate is reuiqred" )]
    public DateOnly? DepartureDate { get; set; }

    [Required( ErrorMessage = "ArrivalTime is required" )]
    public TimeOnly? ArrivalTime { get; set; }

    [Required( ErrorMessage = "DepartureTime is required" )]
    public TimeOnly? DepartureTime { get; set; }

    [Required( ErrorMessage = "GuestName is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "GuestName length must be in range [1;255]" )]
    public string GuestName { get; set; } = null!;

    [Required( ErrorMessage = "GuestPhoneNumber is required" )]
    [Phone]
    public string GuestPhoneNumber { get; set; } = null!;

    [Required( ErrorMessage = "GuestCount is required" )]
    [Range( 1, 10, ErrorMessage = "GuestCount must be in range [1;10]" )]
    public int? GuestCount { get; set; }
}
