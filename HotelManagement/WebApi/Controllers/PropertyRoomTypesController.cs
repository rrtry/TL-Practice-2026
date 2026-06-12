using Domain.Services;
using HotelManagement.WebApi.Dto;
using HotelManagement.WebApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers;

[ApiController]
[Route( "api/properties/{propertyId}/roomtypes" )]
public class PropertyRoomTypesController : ControllerBase
{
    private readonly IRoomTypeService _roomTypeService;

    public PropertyRoomTypesController( IRoomTypeService roomTypeService )
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

    [HttpPost]
    public async Task<IActionResult> Create( Guid propertyId, [FromBody] CreateRoomTypeRequest request )
    {
        var roomType = RoomTypeMapper.MapCreateRequestToEntity( request );
        var created = await _roomTypeService.CreateRoomTypeAsync( propertyId, roomType );

        return CreatedAtAction(
            nameof( RoomTypesController.GetById ),
            "RoomTypes",
            new { id = created.Id },
            RoomTypeMapper.MapEntityToResponse( created ) );
    }
}