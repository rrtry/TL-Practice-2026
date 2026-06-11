using System.Net;
using System.Net.Http.Json;
using HotelManagement.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Tests.Controllers;

public class SearchControllerTests : ControllerTestBase
{
    public SearchControllerTests( CustomWebApplicationFactory factory ) : base( factory ) { }

    [Fact]
    public async Task Search_ValidCriteria_ReturnsMatchingRoomTypes()
    {
        // Arrange
        var property1 = await CreatePropertyAsync( "Hotel Paris", city: "Paris" );
        var roomType1 = await CreateRoomTypeAsync( property1.Id, "Double", dailyPrice: 100, availableRooms: 2, maxGuests: 4 );
        var property2 = await CreatePropertyAsync( "Hotel Lyon", city: "Lyon" );
        var roomType2 = await CreateRoomTypeAsync( property2.Id, "Single", dailyPrice: 80, availableRooms: 1, maxGuests: 1 );

        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 5 ) );
        var departure = arrival.AddDays( 3 ); // 100 * 3

        // Act
        var response = await _client.GetAsync( $"/api/search?city=Paris&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=2" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Equal( HttpStatusCode.OK, response.StatusCode );
        Assert.Single( results );
        Assert.Equal( "Hotel Paris", results![ 0 ].PropertyName );
        Assert.Equal( "Double", results[ 0 ].RoomTypeName );
        Assert.Equal( 100m, results[ 0 ].DailyPrice );
        Assert.Equal( 300m, results[ 0 ].TotalForStay ); // 100 * 3
        Assert.Equal( 2, results[ 0 ].AvailableRooms );
    }

    [Fact]
    public async Task Search_GuestsExceedRoomCapacity_ExcludesRoomType()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Test Hotel", city: "TestCity" );
        var smallRoom = await CreateRoomTypeAsync( property.Id, "Single", maxGuests: 1 );
        var largeRoom = await CreateRoomTypeAsync( property.Id, "Family", maxGuests: 4 );

        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=TestCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=3" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Single( results );
        Assert.Equal( "Family", results![ 0 ].RoomTypeName );
    }

    [Fact]
    public async Task Search_MaxPriceFilter_ExcludesExpensiveRooms()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Price Hotel", city: "PriceCity" );
        var cheapRoom = await CreateRoomTypeAsync( property.Id, "Economy", dailyPrice: 50 );
        var expensiveRoom = await CreateRoomTypeAsync( property.Id, "Luxury", dailyPrice: 200 );

        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=PriceCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1&maxPrice=100" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Single( results );
        Assert.Equal( "Economy", results![ 0 ].RoomTypeName );
    }

    [Fact]
    public async Task Search_RoomFullyBooked_ExcludesRoomType()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Busy Hotel", city: "BusyCity" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Popular", availableRooms: 1 );

        var arrival = DateTime.Today.AddDays( 10 );
        var departure = arrival.AddDays( 3 );

        // Create
        await CreateReservationAsync( property.Id, roomType.Id, arrival, departure );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=BusyCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Empty( results );
    }

    [Fact]
    public async Task Search_PartialAvailability_ShowsRemainingRooms()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Partial Hotel", city: "PartialCity" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Standard", availableRooms: 3 );

        var arrival = DateTime.Today.AddDays( 5 );
        var departure = arrival.AddDays( 2 );

        // Book 2/3
        await CreateReservationAsync( property.Id, roomType.Id, arrival, departure );
        await CreateReservationAsync( property.Id, roomType.Id, arrival, departure );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=PartialCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Single( results );
        Assert.Equal( 1, results![ 0 ].AvailableRooms );
        Assert.Equal( roomType.DailyPrice * 2, results[ 0 ].TotalForStay );
    }

    [Fact]
    public async Task Search_CityNotFound_ReturnsEmptyList()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Some Hotel", city: "ExistingCity" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Room" );
        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=NonExistentCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Empty( results );
    }

    [Fact]
    public async Task Search_NoMatchingRoomType_ReturnsEmptyList()
    {
        // Arrange
        var property = await CreatePropertyAsync( "NoMatch Hotel", city: "MatchCity" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Small", maxGuests: 2 );
        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=MatchCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=5" );
        var results = await response.Content.ReadFromJsonAsync<List<SearchResultResponse>>();

        // Assert
        Assert.Empty( results );
    }

    [Fact]
    public async Task Search_MissingCity_ReturnsBadRequest()
    {
        // Arrange
        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act – no city
        var response = await _client.GetAsync( $"/api/search?arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1" );

        // Assert
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Contains( "City", problem?.Errors.Keys );
    }

    [Fact]
    public async Task Search_InvalidDateRange_ReturnsBadRequest()
    {
        // Arrange
        var property = await CreatePropertyAsync( "Date Hotel", city: "DateCity" );
        var roomType = await CreateRoomTypeAsync( property.Id, "Room" );

        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 5 ) );
        var departure = arrival.AddDays( -2 );

        // Act
        var response = await _client.GetAsync( $"/api/search?city=DateCity&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1" );

        // Assert – InvalidDateRangeException
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var error = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        Assert.Contains( $"Arrival date ({arrival:d}) must be before departure date ({departure:d}).", error?.Title );
    }

    [Fact]
    public async Task Search_MissingArrivalDate_ReturnsBadRequest()
    {
        // Act – no arrivalDate
        var response = await _client.GetAsync( "/api/search?city=Paris&departureDate=2026-06-10&guests=1" );
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
    }

    [Fact]
    public async Task Search_MissingDepartureDate_ReturnsBadRequest()
    {
        // Act – no departureDate
        var response = await _client.GetAsync( "/api/search?city=Paris&arrivalDate=2026-06-05&guests=1" );
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
    }

    [Fact]
    public async Task Search_GuestsLessThanOne_ReturnsBadRequest()
    {
        // Arrange
        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act – guests = 0
        var response = await _client.GetAsync( $"/api/search?city=Paris&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=0" );
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Contains( "Guests", problem?.Errors.Keys );
    }

    [Fact]
    public async Task Search_NegativeMaxPrice_ReturnsBadRequest()
    {
        // Arrange
        var arrival = DateOnly.FromDateTime( DateTime.Today.AddDays( 1 ) );
        var departure = arrival.AddDays( 2 );

        // Act – maxPrice = -10
        var response = await _client.GetAsync( $"/api/search?city=Paris&arrivalDate={arrival:yyyy-MM-dd}&departureDate={departure:yyyy-MM-dd}&guests=1&maxPrice=-10" );
        Assert.Equal( HttpStatusCode.BadRequest, response.StatusCode );
        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.Contains( "MaxPrice", problem?.Errors.Keys );
    }
}
