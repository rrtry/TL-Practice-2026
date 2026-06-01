using Domain.Entities;
using HotelManagement.Dto;

namespace HotelManagement.Mappers;

public static class ReservationMapper
{
    public static Reservation MapCreateRequestToEntity( CreateReservationRequest request )
    {
        return new Reservation
        {
            PropertyId = request.PropertyId!.Value,
            RoomTypeId = request.RoomTypeId!.Value,
            ArrivalDate = request.ArrivalDate!.Value,
            DepartureDate = request.DepartureDate!.Value,
            ArrivalTime = request.ArrivalTime!.Value,
            DepartureTime = request.DepartureTime!.Value,
            GuestPhoneNumber = request.GuestPhoneNumber,
            GuestName = request.GuestName,
            GuestCount = request.GuestCount!.Value
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
