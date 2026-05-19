using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync( Reservation reservation, decimal dailyPrice, int nights );
    Task<IEnumerable<Reservation>> GetFilteredReservationsAsync( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName );
    Task<Reservation?> GetReservationByIdAsync( Guid id );
    Task CancelReservationAsync( Guid id );
}
