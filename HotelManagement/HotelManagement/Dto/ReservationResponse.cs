namespace HotelManagement.Dto;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public TimeOnly ArrivalTime { get; set; }
    public TimeOnly DepartureTime { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string GuestPhoneNumber { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; } = string.Empty;
}
