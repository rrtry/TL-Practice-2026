using System.Net;
using System.Net.Http.Json;
using HotelManagement.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Tests.Controllers;

public class RoomTypesControllerTest : ControllerTestBase
{
    public RoomTypesControllerTest( CustomWebApplicationFactory factory ) : base( factory ) { }

    [Fact]
    public async Task CreateRoomType_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Test Property" );
        var request = new CreateRoomTypeRequest
        {
            Name = "Deluxe Suite",
            DailyPrice = 250m,
            Currency = "USD",
            MinPersonCount = 2,
            MaxPersonCount = 4,
            AvailableRoomsCount = 3
        };

        // Act
        var response = await _client.PostAsJsonAsync( $"/api/properties/{property.Id}/roomtypes", request );
        var created = await response.Content.ReadFromJsonAsync<RoomTypeResponse>();

        // Assert
        Assert.Equal( HttpStatusCode.Created, response.StatusCode );
        Assert.NotNull( created );
        Assert.Equal( "Deluxe Suite", created.Name );
        Assert.Equal( property.Id, created.PropertyId );
        Assert.Equal( 250m, created.DailyPrice );
        Assert.Equal( 3, created.AvailableRoomsCount );
    }

    [Fact]
    public async Task CreateRoomType_PropertyNotFound_ReturnsNotFound()
    {
        // Arrange
        var nonExistentPropertyId = Guid.NewGuid();
        var request = new CreateRoomTypeRequest
        {
            Name = "Standard",
            DailyPrice = 100m,
            Currency = "EUR",
            MinPersonCount = 1,
            MaxPersonCount = 2,
            AvailableRoomsCount = 5
        };

        // Act
        var response = await _client.PostAsJsonAsync( $"/api/properties/{nonExistentPropertyId}/roomtypes", request );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Contains( "Property", error?.Title );
    }

    [Fact]
    public async Task CreateRoomType_InvalidDto_ReturnsBadRequest()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Property" );
        var invalidRequest = new CreateRoomTypeRequest
        {
            Name = "",          // MinLength 1
            DailyPrice = -10,   // non-negative
            Currency = "US",    // exact length 3
            MinPersonCount = 0, // >= 1
            MaxPersonCount = 0,
            AvailableRoomsCount = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync( $"/api/properties/{property.Id}/roomtypes", invalidRequest );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull( problem );
        Assert.Contains( "Name", problem.Errors.Keys );
        Assert.Contains( "DailyPrice", problem.Errors.Keys );
        Assert.Contains( "Currency", problem.Errors.Keys );
        Assert.Contains( "MinPersonCount", problem.Errors.Keys );
        Assert.Contains( "MaxPersonCount", problem.Errors.Keys );
        Assert.Contains( "AvailableRoomsCount", problem.Errors.Keys );
    }

    [Fact]
    public async Task GetRoomTypesByProperty_ReturnsList()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel with rooms" );
        var rt1 = await CreateRoomTypeAsync( property.Id, "Standard" );
        var rt2 = await CreateRoomTypeAsync( property.Id, "Suite" );

        // Act
        var response = await _client.GetAsync( $"/api/properties/{property.Id}/roomtypes" );
        var roomTypes = await response.Content.ReadFromJsonAsync<List<RoomTypeResponse>>();

        // Assert
        Assert.Equal( HttpStatusCode.OK, response.StatusCode );
        Assert.Equal( 2, roomTypes?.Count );
        Assert.Contains( roomTypes, r => r.Name == "Standard" );
        Assert.Contains( roomTypes, r => r.Name == "Suite" );
    }

    [Fact]
    public async Task GetRoomTypeById_Existing_ReturnsOk()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Property" );
        var created = await CreateRoomTypeAsync( property.Id, "Single" );

        // Act
        var response = await _client.GetAsync( $"/api/roomtypes/{created.Id}" );
        var roomType = await response.Content.ReadFromJsonAsync<RoomTypeResponse>();

        // Assert
        Assert.Equal( HttpStatusCode.OK, response.StatusCode );
        Assert.Equal( created.Id, roomType?.Id );
        Assert.Equal( "Single", roomType?.Name );
    }

    [Fact]
    public async Task GetRoomTypeById_NonExisting_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync( $"/api/roomtypes/{Guid.NewGuid()}" );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
    }

    [Fact]
    public async Task UpdateRoomType_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Property" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Old Name" );
        var updateRequest = new UpdateRoomTypeRequest
        {
            Name = "New Name",
            DailyPrice = 300m,
            Currency = "GBP",
            MinPersonCount = 2,
            MaxPersonCount = 5,
            AvailableRoomsCount = 10
        };

        // Act
        var response = await _client.PutAsJsonAsync( $"/api/roomtypes/{roomType.Id}", updateRequest );

        // Assert
        Assert.Equal( HttpStatusCode.NoContent, response.StatusCode );

        // Verify
        var getResponse = await _client.GetAsync( $"/api/roomtypes/{roomType.Id}" );
        var updated = await getResponse.Content.ReadFromJsonAsync<RoomTypeResponse>();

        Assert.Equal( "New Name", updated?.Name );
        Assert.Equal( 300m, updated?.DailyPrice );
        Assert.Equal( "GBP", updated?.Currency );
    }

    [Fact]
    public async Task UpdateRoomType_NonExisting_ReturnsNotFound()
    {
        // Arrange
        var updateRequest = new UpdateRoomTypeRequest
        {
            Name = "Any",
            DailyPrice = 100,
            Currency = "USD",
            MinPersonCount = 1,
            MaxPersonCount = 2,
            AvailableRoomsCount = 1
        };

        // Act
        var response = await _client.PutAsJsonAsync( $"/api/roomtypes/{Guid.NewGuid()}", updateRequest );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
    }

    [Fact]
    public async Task UpdateRoomType_InvalidDto_ReturnsBadRequest()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Property" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Original" );
        var invalidUpdate = new UpdateRoomTypeRequest
        {
            Name = "", // invalid
            DailyPrice = -5,
            Currency = "INVALID",
            MinPersonCount = 0,
            MaxPersonCount = 0,
            AvailableRoomsCount = 0
        };

        // Act
        var response = await _client.PutAsJsonAsync( $"/api/roomtypes/{roomType.Id}", invalidUpdate );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull( problem );
        Assert.Contains( "Name", problem.Errors );
        Assert.Contains( "DailyPrice", problem.Errors );
        Assert.Contains( "Currency", problem.Errors );
    }

    [Fact]
    public async Task DeleteRoomType_WithoutReservations_ReturnsNoContent()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Property" );
        var roomType = await CreateRoomTypeAsync( property.Id, "ToDelete" );

        // Act
        var response = await _client.DeleteAsync( $"/api/roomtypes/{roomType.Id}" );

        // Assert
        Assert.Equal( HttpStatusCode.NoContent, response.StatusCode );

        // Verify deletion
        var getResponse = await _client.GetAsync( $"/api/roomtypes/{roomType.Id}" );
        Assert.Equal( HttpStatusCode.NotFound, getResponse.StatusCode );
    }

    [Fact]
    public async Task DeleteRoomType_WithExistingReservations_ReturnsBadRequest()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Popular", availableRooms: 2 );
        await CreateReservationAsync( property.Id, roomType.Id, DateTime.Today.AddDays( 1 ), DateTime.Today.AddDays( 3 ) );

        // Act
        var response = await _client.DeleteAsync( $"/api/roomtypes/{roomType.Id}" );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var roomTypeId = roomType.Id;

        Assert.Contains( $"Cannot delete room type '{roomTypeId}' because it has existing reservations.", error?.Title );
    }

    [Fact]
    public async Task DeleteRoomType_NonExisting_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync( $"/api/roomtypes/{Guid.NewGuid()}" );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
    }
}