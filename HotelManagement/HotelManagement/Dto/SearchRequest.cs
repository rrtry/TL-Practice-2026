namespace HotelManagement.Dto;

public class SearchRequest
{
    public string? City { get; set; }
    public DateOnly ArrivalDate { get; set; }
    public DateOnly DepartureDate { get; set; }
    public int Guests { get; set; }
    public decimal? MaxPrice { get; set; }
}