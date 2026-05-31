using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync( Reservation reservation );
    Task<IEnumerable<Reservation>> GetFilteredReservationsAsync( Guid? propertyId, DateOnly? fromDate, DateOnly? toDate, string? guestName );
    Task<Reservation?> GetReservationByIdAsync( Guid id );
    Task CancelReservationAsync( Guid id );
}
