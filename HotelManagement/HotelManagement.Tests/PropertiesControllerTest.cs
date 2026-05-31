using System.Net;
using System.Net.Http.Json;
using HotelManagement.Dto;

namespace HotelManagement.Tests.Controllers;

public class PropertiesControllerTests : IntegrationTestBase
{
    public PropertiesControllerTests( CustomWebApplicationFactory factory ) : base( factory ) { }

    [Fact]
    public async Task Create_ValidProperty_ReturnsCreated()
    {
        // Arrange
        var request = new CreatePropertyRequest
        {
            Name = "Moscow Hotel",
            City = "Moscow",
            Country = "RUS",
            Address = "Tverskaya 1",
            Latitude = 55.75,
            Longitude = 37.62
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/properties", request );
        var created = await response.Content.ReadFromJsonAsync<PropertyResponse>();

        // Assert
        Assert.Equal( HttpStatusCode.Created, response.StatusCode );
        Assert.NotNull( created?.Id );
        Assert.Equal( "Moscow Hotel", created.Name );
        Assert.Equal( "Moscow", created.City );
    }

    [Fact]
    public async Task GetById_ExistingProperty_ReturnsOk()
    {
        var createRequest = new CreatePropertyRequest
        {
            Name = "SBP Hotel",
            City = "Saint-Petersburg",
            Country = "RUS",
            Address = "Tverskaya 1",
            Latitude = 55.75,
            Longitude = 37.62
        };

        var createResponse = await _client.PostAsJsonAsync( "/api/properties", createRequest );
        var created = await createResponse.Content.ReadFromJsonAsync<PropertyResponse>();

        // Act
        var getResponse = await _client.GetAsync( $"/api/properties/{created!.Id}" );
        var property = await getResponse.Content.ReadFromJsonAsync<PropertyResponse>();

        // Assert
        Assert.Equal( HttpStatusCode.OK, getResponse.StatusCode );
        Assert.Equal( created.Id, property!.Id );
        Assert.Equal( "SBP Hotel", property.Name );
    }

    [Fact]
    public async Task Delete_ExistingProperty_ReturnsNoContent()
    {
        // Arrange
        var createRequest = new CreatePropertyRequest
        {
            Name = "SBP Hotel",
            City = "Saint-Petersburg",
            Country = "RUS",
            Address = "Tverskaya 1",
            Latitude = 55.75,
            Longitude = 37.62
        };

        var createResponse = await _client.PostAsJsonAsync( "/api/properties", createRequest );
        var created = await createResponse.Content.ReadFromJsonAsync<PropertyResponse>();

        // Act
        var deleteResponse = await _client.DeleteAsync( $"/api/properties/{created!.Id}" );

        // Assert
        Assert.Equal( HttpStatusCode.NoContent, deleteResponse.StatusCode );

        // Verify
        var getResponse = await _client.GetAsync( $"/api/properties/{created.Id}" );
        Assert.Equal( HttpStatusCode.NotFound, getResponse.StatusCode );
    }

    [Fact]
    public async Task Delete_PropertyWithRoomTypes_ReturnsBadRequest()
    {
        // Arrange
        var propertyRequest = new CreatePropertyRequest
        {
            Name = "Hotel With Rooms",
            City = "Saint-Petersburg",
            Country = "RUS",
            Address = "Tverskaya 1",
            Latitude = 55.75,
            Longitude = 37.62
        };

        var response = await _client.PostAsJsonAsync( "/api/properties", propertyRequest );
        var property = await response.Content.ReadFromJsonAsync<PropertyResponse>();

        var roomType = new CreateRoomTypeRequest
        {
            Name = "Standard",
            Currency = "RUB",
            DailyPrice = 100m,
            MinPersonCount = 1,
            MaxPersonCount = 2,
            AvailableRoomsCount = 5
        };

        await _client.PostAsJsonAsync( $"/api/properties/{property.Id}/roomtypes", roomType );

        // Act
        var deleteResponse = await _client.DeleteAsync( $"/api/properties/{property.Id}" );

        // Assert
        Assert.Equal( HttpStatusCode.Conflict, deleteResponse.StatusCode );
        var errorText = await deleteResponse.Content.ReadAsStringAsync();
        Assert.Contains( "Cannot delete property with existing room types", errorText );
    }
}