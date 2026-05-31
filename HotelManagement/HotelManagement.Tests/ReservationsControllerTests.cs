using System.Net;
using System.Net.Http.Json;
using HotelManagement.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Tests.Controllers;

public class ReservationsControllerTests : IntegrationTestBase
{
    public ReservationsControllerTests( CustomWebApplicationFactory factory ) : base( factory ) { }

    [Fact]
    public async Task CreateReservation_ValidRequest_ReturnsCreatedWithTotal()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Beach Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Standard", dailyPrice: 150m, availableRooms: 3 );
        var request = new CreateReservationRequest
        {
            PropertyId = property.Id,
            RoomTypeId = roomType.Id,
            ArrivalDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 10 ) ),
            DepartureDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 15 ) ),
            ArrivalTime = new TimeOnly( 12, 0 ),
            DepartureTime = new TimeOnly( 14, 0 ),
            GuestName = "John Doe",
            GuestPhoneNumber = "+1234567890",
            GuestCount = 2
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", request );
        var reservation = await response.Content.ReadFromJsonAsync<ReservationResponse>();

        // Assert
        Assert.Equal( HttpStatusCode.Created, response.StatusCode );
        Assert.NotNull( reservation );

        Assert.Equal( property.Id, reservation.PropertyId );
        Assert.Equal( roomType.Id, reservation.RoomTypeId );
        Assert.Equal( 5 * 150m, reservation.Total );
        Assert.Equal( "USD", reservation.Currency );
        Assert.Equal( "John Doe", reservation.GuestName );
    }

    [Fact]
    public async Task CreateReservation_WhenRoomFullyBooked_ReturnsConflict()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Single", availableRooms: 1 );

        var arrivalDateTime = DateTime.Today.AddDays( 5 );
        var departureDateTime = DateTime.Today.AddDays( 7 );

        // First reservation
        var created = await CreateReservationAsync( property.Id, roomType.Id, arrivalDateTime, departureDateTime );

        // Verify
        var getResponse = await _client.GetAsync( $"/api/reservations/{created.Id}" );
        var reservation = await getResponse.Content.ReadFromJsonAsync<ReservationResponse>();
        Assert.Equal( created.Id, reservation.Id );

        // Second reservation
        var secondRequest = new CreateReservationRequest
        {
            PropertyId = property.Id,
            RoomTypeId = roomType.Id,
            ArrivalDate = DateOnly.FromDateTime( arrivalDateTime ),
            DepartureDate = DateOnly.FromDateTime( departureDateTime ),
            ArrivalTime = new TimeOnly( 12, 0 ),
            DepartureTime = new TimeOnly( 14, 0 ),
            GuestName = "Second Guest",
            GuestPhoneNumber = "+123456789",
            GuestCount = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", secondRequest );

        // Assert
        Assert.Equal( HttpStatusCode.Conflict, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Contains( "No available rooms", error?.Title );
    }

    [Fact]
    public async Task CreateReservation_InvalidDates_ReturnsBadRequest()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Room" );
        var request = new CreateReservationRequest
        {
            PropertyId = property.Id,
            RoomTypeId = roomType.Id,
            ArrivalDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 5 ) ),
            DepartureDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 3 ) ),
            ArrivalTime = new TimeOnly( 12, 0 ),
            DepartureTime = new TimeOnly( 14, 0 ),
            GuestName = "Invalid",
            GuestPhoneNumber = "123",
            GuestCount = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", request );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        var arrival = request.ArrivalDate;
        var departure = request.DepartureDate;
        var title = $"Arrival date ({arrival:d}) must be before departure date ({departure:d}).";

        Assert.Equal( title, problem?.Title );
    }

    [Fact]
    public async Task CreateReservation_GuestCountOutOfRange_ReturnsBadRequest()
    {
        // Arrange: 1-2 guests
        var property = await CreatePropertyAsync( "Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Small Room", maxGuests: 2 );
        var request = new CreateReservationRequest
        {
            PropertyId = property.Id,
            RoomTypeId = roomType.Id,
            ArrivalDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) ),
            DepartureDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 2 ) ),
            ArrivalTime = new TimeOnly( 12, 0 ),
            DepartureTime = new TimeOnly( 14, 0 ),
            GuestName = "Too Many",
            GuestPhoneNumber = "123",
            GuestCount = 5 // exceeds max
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", request );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Contains( "Guest count must be between", error?.Title );
    }

    [Fact]
    public async Task CreateReservation_PropertyNotFound_ReturnsNotFound()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Temp" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Temp" );
        var request = new CreateReservationRequest
        {
            PropertyId = Guid.NewGuid(),
            RoomTypeId = roomType.Id,
            ArrivalDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) ),
            DepartureDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 3 ) ),
            ArrivalTime = new TimeOnly( 12, 0 ),
            DepartureTime = new TimeOnly( 14, 0 ),
            GuestName = "Guest",
            GuestPhoneNumber = "123",
            GuestCount = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", request );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var propertyId = request.PropertyId;

        Assert.Equal( $"Property with id '{propertyId}' was not found.", error?.Title );
    }

    [Fact]
    public async Task CreateReservation_RoomTypeNotFound_ReturnsNotFound()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel" );
        var request = new CreateReservationRequest
        {
            PropertyId = property.Id,
            RoomTypeId = Guid.NewGuid(),
            ArrivalDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) ),
            DepartureDate = DateOnly.FromDateTime( DateTime.Today.AddDays( 3 ) ),
            ArrivalTime = new TimeOnly( 12, 0 ),
            DepartureTime = new TimeOnly( 14, 0 ),
            GuestName = "Guest",
            GuestPhoneNumber = "123",
            GuestCount = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", request );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        var roomTypeId = request.RoomTypeId;

        Assert.Equal( $"Room type with id '{roomTypeId}' was not found.", error?.Title );
    }

    [Fact]
    public async Task CreateReservation_InvalidDto_ReturnsBadRequest()
    {
        // Arrange
        var invalidRequest = new CreateReservationRequest
        {
            GuestName = "",
            GuestPhoneNumber = "not-a-phone",
            GuestCount = 0
        };

        // Act
        var response = await _client.PostAsJsonAsync( "/api/reservations", invalidRequest );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull( problem );
        Assert.Contains( "GuestName", problem.Errors );
        Assert.Contains( "GuestPhoneNumber", problem.Errors );
        Assert.Contains( "GuestCount", problem.Errors );
    }

    [Fact]
    public async Task GetReservations_NoFilters_ReturnsAll()
    {
        // Arrange: create two reservations
        var prop1 = await CreatePropertyAsync( "Hotel A" );
        var rt1 = await CreateRoomTypeAsync( prop1.Id, "Type A" );
        var res1 = await CreateReservationAsync( prop1.Id, rt1.Id );

        var prop2 = await CreatePropertyAsync( "Hotel B" );
        var rt2 = await CreateRoomTypeAsync( prop2.Id, "Type B" );
        var res2 = await CreateReservationAsync( prop2.Id, rt2.Id );

        // Act
        var response = await _client.GetAsync( "/api/reservations" );
        var reservations = await response.Content.ReadFromJsonAsync<List<ReservationResponse>>();

        // Assert
        Assert.Equal( HttpStatusCode.OK, response.StatusCode );
        Assert.NotNull( reservations );
        Assert.Contains( reservations, r => r.Id == res1.Id );
        Assert.Contains( reservations, r => r.Id == res2.Id );
    }

    [Fact]
    public async Task GetReservations_FilterByPropertyId_ReturnsFiltered()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Filter Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Room" );
        var reservation = await CreateReservationAsync( property.Id, roomType.Id );

        // Act
        var response = await _client.GetAsync( $"/api/reservations?propertyId={property.Id}" );
        var reservations = await response.Content.ReadFromJsonAsync<List<ReservationResponse>>();

        // Assert
        Assert.Single( reservations );
        Assert.Equal( property.Id, reservations!.First().PropertyId );
    }

    [Fact]
    public async Task GetReservationById_Existing_ReturnsOk()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Room" );
        var created = await CreateReservationAsync( property.Id, roomType.Id );

        // Act
        var response = await _client.GetAsync( $"/api/reservations/{created.Id}" );
        var reservation = await response.Content.ReadFromJsonAsync<ReservationResponse>();

        // Assert
        Assert.Equal( HttpStatusCode.OK, response.StatusCode );
        Assert.Equal( created.Id, reservation?.Id );
    }

    [Fact]
    public async Task GetReservationById_NonExisting_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync( $"/api/reservations/{Guid.NewGuid()}" );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
    }

    [Fact]
    public async Task CancelReservation_Existing_ReturnsNoContent()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Hotel" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Room" );
        var reservation = await CreateReservationAsync( property.Id, roomType.Id );

        // Act
        var response = await _client.DeleteAsync( $"/api/reservations/{reservation.Id}" );

        // Assert
        Assert.Equal( HttpStatusCode.NoContent, response.StatusCode );

        // Verify
        var getResponse = await _client.GetAsync( $"/api/reservations/{reservation.Id}" );
        Assert.Equal( HttpStatusCode.NotFound, getResponse.StatusCode );
    }

    [Fact]
    public async Task CancelReservation_NonExisting_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync( $"/api/reservations/{Guid.NewGuid()}" );

        // Assert
        Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
    }
}