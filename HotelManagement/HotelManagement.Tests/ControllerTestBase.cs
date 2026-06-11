using System.Net.Http.Json;
using HotelManagement.WebApi.Dto;

namespace HotelManagement.Tests;

public class ControllerTestBase : IClassFixture<CustomWebApplicationFactory>
{
    protected readonly HttpClient _client;
    protected readonly CustomWebApplicationFactory _factory;

    public ControllerTestBase( CustomWebApplicationFactory factory )
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    protected async Task<PropertyResponse> CreatePropertyAsync( string name, string city = "TestCity" )
    {
        var request = new CreatePropertyRequest
        {
            Name = name,
            City = city,
            Country = "RUS",
            Address = "Test Address",
            Latitude = 55.75,
            Longitude = 37.62
        };
        var response = await _client.PostAsJsonAsync( "/api/properties", request );

        return await response.Content.ReadFromJsonAsync<PropertyResponse>();
    }

    protected async Task<RoomTypeResponse> CreateRoomTypeAsync( Guid propertyId, string name, decimal dailyPrice = 100m, int availableRooms = 5, int maxGuests = 4 )
    {
        var request = new CreateRoomTypeRequest
        {
            Name = name,
            DailyPrice = dailyPrice,
            Currency = "USD",
            MinPersonCount = 1,
            MaxPersonCount = maxGuests,
            AvailableRoomsCount = availableRooms
        };

        var response = await _client.PostAsJsonAsync( $"/api/properties/{propertyId}/roomtypes", request );

        return await response.Content.ReadFromJsonAsync<RoomTypeResponse>();
    }

    protected async Task<ReservationResponse> CreateReservationAsync(
        Guid propertyId,
        Guid roomTypeId,
        DateTime? arrival = null,
        DateTime? departure = null )
    {
        var today = DateTime.Today;
        var arrivalDate = arrival.HasValue
            ? DateOnly.FromDateTime( arrival.Value )
            : DateOnly.FromDateTime( today.AddDays( 1 ) );

        var departureDate = departure.HasValue
            ? DateOnly.FromDateTime( departure.Value )
            : arrivalDate.AddDays( 2 );

        var arrivalTime = new TimeOnly( 14, 0 );
        var departureTime = new TimeOnly( 12, 0 );

        var request = new CreateReservationRequest
        {
            PropertyId = propertyId,
            RoomTypeId = roomTypeId,
            ArrivalDate = arrivalDate,
            DepartureDate = departureDate,
            ArrivalTime = arrivalTime,
            DepartureTime = departureTime,
            GuestName = "Test Guest",
            GuestPhoneNumber = "+1234567890",
            GuestCount = 1
        };
        var response = await _client.PostAsJsonAsync( "/api/reservations", request );

        return await response.Content.ReadFromJsonAsync<ReservationResponse>();
    }
}