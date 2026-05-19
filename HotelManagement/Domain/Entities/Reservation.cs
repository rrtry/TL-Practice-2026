namespace Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid RoomTypeId { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public string ArrivalTime { get; set; } = "14:00";
    public string DepartureTime { get; set; } = "12:00";
    public string GuestName { get; set; } = string.Empty;
    public string GuestPhoneNumber { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string Currency { get; set; } = "RUB";
    public int GuestCount { get; set; }
}
