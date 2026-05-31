using Domain.Entities;
using HotelManagement.Dto;

namespace HotelManagement.Mappers;

public static class ReservationMapper
{
    public static Reservation MapCreateRequestToEntity( CreateReservationRequest request )
    {
        return new Reservation
        {
            PropertyId = request.PropertyId,
            RoomTypeId = request.RoomTypeId,
            ArrivalDate = request.ArrivalDate,
            DepartureDate = request.DepartureDate,
            ArrivalTime = request.ArrivalTime,
            DepartureTime = request.DepartureTime,
            GuestName = request.GuestName,
            GuestPhoneNumber = request.GuestPhoneNumber,
            GuestCount = request.GuestCount
        };
    }

    public static ReservationResponse MapEntityToResponse( Reservation created )
    {
        return new ReservationResponse
        {
            Id = created.Id,
            PropertyId = created.PropertyId,
            RoomTypeId = created.RoomTypeId,
            ArrivalDate = created.ArrivalDate,
            DepartureDate = created.DepartureDate,
            ArrivalTime = created.ArrivalTime,
            DepartureTime = created.DepartureTime,
            GuestName = created.GuestName,
            GuestPhoneNumber = created.GuestPhoneNumber,
            GuestCount = created.GuestCount,
            Total = created.Total,
            Currency = created.Currency
        };
    }
}
