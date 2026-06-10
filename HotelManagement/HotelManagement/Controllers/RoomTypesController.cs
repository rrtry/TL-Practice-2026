using Domain.Services;
using HotelManagement.Dto;
using HotelManagement.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers;

[ApiController]
[Route( "api/properties/{propertyId}/roomtypes" )]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypesController( IRoomTypeService roomTypeService )
    {
        _roomTypeService = roomTypeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByProperty( Guid propertyId )
    {
        var roomTypes = await _roomTypeService.GetRoomTypesByPropertyIdAsync( propertyId );
        var responses = roomTypes.Select( rt => RoomTypeMapper.MapEntityToResponse( rt ) );

        return Ok( responses );
    }

    [HttpGet( "~/api/roomtypes/{id}" )]
    public async Task<IActionResult> GetById( Guid id )
    {
        var roomType = await _roomTypeService.GetRoomTypeByIdAsync( id );

        return Ok( RoomTypeMapper.MapEntityToResponse( roomType ) );
    }

    [HttpPost]
    public async Task<IActionResult> Create( Guid propertyId, [FromBody] CreateRoomTypeRequest request )
    {
        var roomType = RoomTypeMapper.MapCreateRequestToEntity( request );
        var created = await _roomTypeService.CreateRoomTypeAsync( propertyId, roomType );

        return CreatedAtAction( nameof( GetById ), new { id = created.Id }, RoomTypeMapper.MapEntityToResponse( created ) );
    }

    [HttpPut( "~/api/roomtypes/{id}" )]
    public async Task<IActionResult> Update( Guid id, [FromBody] UpdateRoomTypeRequest request )
    {
        var existing = await _roomTypeService.GetRoomTypeByIdAsync( id );

        RoomTypeMapper.Update( existing, request );
        await _roomTypeService.UpdateRoomTypeAsync( existing );

        return NoContent();
    }

    [HttpDelete( "~/api/roomtypes/{id}" )]
    public async Task<IActionResult> Delete( Guid id )
    {
        await _roomTypeService.DeleteRoomTypeAsync( id );

        return NoContent();
    }
}