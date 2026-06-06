using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync( Guid id );
    Task AddAsync( Reservation reservation );
    Task DeleteAsync( Guid id );
    Task<int> GetOverlappingReservationsCountAsync( Guid roomTypeId, DateOnly arrival, DateOnly departure );
    Task<IEnumerable<Reservation>> GetFilteredAsync( ReservationFilter filter );
}