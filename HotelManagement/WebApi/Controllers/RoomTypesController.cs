using Domain.Services;
using HotelManagement.WebApi.Dto;
using HotelManagement.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers;

[ApiController]
[Route( "api/roomtypes" )]
public class RoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public RoomTypesController( IRoomTypeService roomTypeService )
    {
        _roomTypeService = roomTypeService;
    }

    [HttpGet( "{id}" )]
    public async Task<IActionResult> GetById( Guid id )
    {
        var roomType = await _roomTypeService.GetRoomTypeByIdAsync( id );
        return Ok( RoomTypeMapper.MapEntityToResponse( roomType ) );
    }

    [HttpPut( "{id}" )]
    public async Task<IActionResult> Update( Guid id, [FromBody] UpdateRoomTypeRequest request )
    {
        await _roomTypeService.UpdateRoomTypeAsync( id, roomType => RoomTypeMapper.Update( roomType, request ) );
        return NoContent();
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Delete( Guid id )
    {
        await _roomTypeService.DeleteRoomTypeAsync( id );
        return NoContent();
    }
}