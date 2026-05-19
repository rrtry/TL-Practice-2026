namespace HotelManagement.Dto;

public class CreateRoomTypeRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = "USD";
    public int MinPersonCount { get; set; }
    public int MaxPersonCount { get; set; }
    public int AvailableRoomsCount { get; set; }
    public List<string> Services { get; set; } = new();
    public List<string> Amenities { get; set; } = new();
}
