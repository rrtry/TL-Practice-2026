using Domain.Interfaces.Services;
using HotelManagement.Dto;
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
        if ( request.ArrivalDate == default || request.DepartureDate == default )
        {
            return BadRequest( "ArrivalDate and DepartureDate are required." );
        }

        if ( request.Guests <= 0 )
        {
            return BadRequest( "Guests must be at least 1." );
        }

        try
        {
            var results = await _searchService.SearchAvailableAsync( request.City, request.ArrivalDate, request.DepartureDate, request.Guests, request.MaxPrice );

            var response = results.Select( r => new SearchResultResponse
            {
                PropertyId = r.PropertyId,
                PropertyName = r.PropertyName,
                City = r.City,
                RoomTypeId = r.RoomTypeId,
                RoomTypeName = r.RoomTypeName,
                DailyPrice = r.DailyPrice,
                Currency = r.Currency,
                TotalForStay = r.TotalForStay,
                AvailableRooms = r.AvailableRooms
            } );

            return Ok( response );
        }
        catch ( ArgumentException ex )
        {
            return BadRequest( ex.Message );
        }
    }
}