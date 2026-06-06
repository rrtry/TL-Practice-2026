using Domain.Services;
using HotelManagement.Dto;
using HotelManagement.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.WebApi.Controllers;

[ApiController]
[Route( "api/properties" )]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;

    public PropertiesController( IPropertyService propertyService )
    {
        _propertyService = propertyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var properties = await _propertyService.GetAllPropertiesAsync();
        var responses = properties.Select( p => PropertyMapper.MapEntityToResponse( p ) );

        return Ok( responses );
    }

    [HttpGet( "{id}" )]
    public async Task<IActionResult> GetById( Guid id )
    {
        var property = await _propertyService.GetPropertyByIdAsync( id );

        if ( property == null )
        {
            return NotFound();
        }

        return Ok( PropertyMapper.MapEntityToResponse( property ) );
    }

    [HttpPost]
    public async Task<IActionResult> Create( [FromBody] CreatePropertyRequest request )
    {
        var property = PropertyMapper.MapCreateRequestToEntity( request );
        var created = await _propertyService.CreatePropertyAsync( property );

        return CreatedAtAction( nameof( GetById ), new { id = created.Id }, PropertyMapper.MapEntityToResponse( created ) );
    }

    [HttpPut( "{id}" )]
    public async Task<IActionResult> Update( Guid id, [FromBody] UpdatePropertyRequest request )
    {
        var existing = await _propertyService.GetPropertyByIdAsync( id );

        if ( existing == null )
        {
            return NotFound();
        }

        PropertyMapper.Update( existing, request );
        await _propertyService.UpdatePropertyAsync( existing );

        return NoContent();
    }

    [HttpDelete( "{id}" )]
    public async Task<IActionResult> Delete( Guid id )
    {
        await _propertyService.DeletePropertyAsync( id );
        return NoContent();
    }
}
