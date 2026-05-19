namespace Domain.Interfaces.Services;

using Domain.Models;

public interface ISearchService
{
    Task<IEnumerable<SearchResultItem>> SearchAvailableAsync( string? city, DateTime arrivalDate, DateTime departureDate, int guests, decimal? maxPrice );
}
