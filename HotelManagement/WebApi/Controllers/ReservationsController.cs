using Domain.Filters;
using Domain.Services;
using HotelManagement.WebApi.Dto;
using HotelManagement.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers;

[ApiController]
[Route( "api/reservations" )]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationsController( IReservationService reservationService )
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<IActionResult> Create( [FromBody] CreateReservationRequest request )
    {
        var reservation = ReservationMapper.MapCreateRequestToEntity( request );
        var created = await _reservationService.CreateReservationAsync( reservation );

        return CreatedAtAction(
            nameof( GetById ),
            new { id = created.Id },
            ReservationMapper.MapEntityToResponse( created )
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll( [FromQuery] ReservationFilter filter )
    {
        var reservations = await _reservationService.GetFilteredReservationsAsync( filter );
        var responses = reservations.Select( ReservationMapper.MapEntityToResponse );

        return Ok( responses );
    }

    [HttpGet( "{id}" )]
    public async Task<IActionResult> GetById( Guid id )
    {
        var reservation = await _reservationService.GetReservationByIdAsync( id );
        return Ok( ReservationMapper.MapEntityToResponse( reservation ) );
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Delete( Guid id )
    {
        await _reservationService.DeleteReservationAsync( id );
        return NoContent();
    }
}