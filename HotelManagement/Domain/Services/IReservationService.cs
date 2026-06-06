using Domain.Entities;
using Domain.Filters;

namespace Domain.Services;

public interface IReservationService
{
    Task<Reservation> CreateReservationAsync( Reservation reservation );
    Task<IEnumerable<Reservation>> GetFilteredReservationsAsync( ReservationFilter filter );
    Task<Reservation?> GetReservationByIdAsync( Guid id );
    Task CancelReservationAsync( Guid id );
}
