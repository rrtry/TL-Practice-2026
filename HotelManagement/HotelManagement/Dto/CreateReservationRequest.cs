using System.ComponentModel.DataAnnotations;

namespace HotelManagement.Dto;

public class CreateReservationRequest
{
    [Required( ErrorMessage = "Reservation PropertyId is required" )]
    public Guid? PropertyId { get; set; }

    [Required( ErrorMessage = "Reservation RoomTypeId is required" )]
    public Guid? RoomTypeId { get; set; }

    [Required( ErrorMessage = "Reservation ArrivalDate is required" )]
    public DateOnly? ArrivalDate { get; set; }

    [Required( ErrorMessage = "Reservation DepartureDate is reuiqred" )]
    public DateOnly? DepartureDate { get; set; }

    [Required( ErrorMessage = "Reservation ArrivalTime is required" )]
    public TimeOnly? ArrivalTime { get; set; }

    [Required( ErrorMessage = "Reservation DepartureTime is required" )]
    public TimeOnly? DepartureTime { get; set; }

    [Required( ErrorMessage = "Reservation GuestName is required" )]
    [StringLength( 255, MinimumLength = 1, ErrorMessage = "Reservation GuestName length must be in range [1;255]" )]
    public string GuestName { get; set; } = null!;

    [Required( ErrorMessage = "Reservation GuestPhoneNumber is required" )]
    [Phone]
    public string GuestPhoneNumber { get; set; } = null!;

    [Required( ErrorMessage = "Reservation GuestCount is required" )]
    [Range( 1, 10, ErrorMessage = "Reservation GuestCount must be in range [1;10]" )]
    public int? GuestCount { get; set; }
}
