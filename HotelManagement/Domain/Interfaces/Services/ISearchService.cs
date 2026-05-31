namespace Domain.Interfaces.Services;

using Domain.Models;

public interface ISearchService
{
    Task<IEnumerable<SearchResultItem>> SearchAvailableAsync( string? city, DateOnly arrivalDate, DateOnly departureDate, int guests, decimal? maxPrice );
}
