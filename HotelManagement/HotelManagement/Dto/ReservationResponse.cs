namespace HotelManagement.Dto;

public class ReservationResponse
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public string ArrivalTime { get; set; } = string.Empty;
    public string DepartureTime { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string GuestPhoneNumber { get; set; } = string.Empty;
    public int GuestCount { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; } = string.Empty;
}
