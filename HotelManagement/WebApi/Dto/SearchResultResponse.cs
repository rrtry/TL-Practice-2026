namespace HotelManagement.WebApi.Dto;

public class SearchResultResponse
{
    public Guid PropertyId { get; set; }
    public string PropertyName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public Guid RoomTypeId { get; set; }
    public string RoomTypeName { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal TotalForStay { get; set; }
    public int AvailableRooms { get; set; }
}
