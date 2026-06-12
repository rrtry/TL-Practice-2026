using Domain.Services;
using HotelManagement.WebApi.Dto;
using HotelManagement.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers;

[ApiController]
[Route( "api/search" )]
public class SearchController : ControllerBase
{
    private readonly ISearchService _searchService;

    public SearchController( ISearchService searchService )
    {
        _searchService = searchService;
    }

    [HttpGet]
    public async Task<IActionResult> Search( [FromQuery] SearchRequest request )
    {
        var filter = SearchMapper.MapSearchRequestToFilter( request );
        var results = await _searchService.SearchAvailableAsync( filter );
        var response = results.Select( SearchMapper.MapSearchResultToResponse );

        return Ok( response );
    }
}