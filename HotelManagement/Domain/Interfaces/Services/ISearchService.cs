namespace Domain.Interfaces.Services;

using Domain.Filters;
using Domain.Models;

public interface ISearchService
{
    Task<IEnumerable<SearchResultItem>> SearchAvailableAsync( SearchFilter filter );
}
