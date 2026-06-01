namespace Domain.Filters;

public class SearchFilter
{
    public string City { get; set; } = string.Empty;

    public DateOnly ArrivalDate { get; set; }

    public DateOnly DepartureDate { get; set; }

    public int Guests { get; set; }

    public decimal? MaxPrice { get; set; }
}