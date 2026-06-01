using Domain.Filters;
using Domain.Interfaces.Services;
using HotelManagement.Dto;
using HotelManagement.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApi.WebApi.Controllers;

[ApiController]
[Route( "api/reservations" )]
public class ReservationsController : ControllerBase
{
    private readonly IReservationService _reservationService;
    private readonly IRoomTypeService _roomTypeService;

    public ReservationsController( IReservationService reservationService, IRoomTypeService roomTypeService )
    {
        _reservationService = reservationService;
        _roomTypeService = roomTypeService;
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

        if ( reservation == null )
        {
            return NotFound();
        }

        return Ok( ReservationMapper.MapEntityToResponse( reservation ) );
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Cancel( Guid id )
    {
        await _reservationService.CancelReservationAsync( id );
        return NoContent();
    }
}