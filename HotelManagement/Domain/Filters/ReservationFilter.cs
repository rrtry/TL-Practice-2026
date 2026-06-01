namespace Domain.Filters;

public class ReservationFilter
{
    public Guid? PropertyId { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }

    public string? GuestName { get; set; }
}
