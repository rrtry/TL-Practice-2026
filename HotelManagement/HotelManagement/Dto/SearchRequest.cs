namespace HotelManagement.Dto;

public class SearchRequest
{
    public string? City { get; set; }
    public DateTime ArrivalDate { get; set; }
    public DateTime DepartureDate { get; set; }
    public int Guests { get; set; }
    public decimal? MaxPrice { get; set; }
}