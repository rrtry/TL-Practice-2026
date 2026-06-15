using Domain.Entities;
using Domain.Filters;

namespace Domain.Repositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdForUpdateAsync( Guid id );
    Task<Reservation?> GetByIdAsync( Guid id );
    Task AddAsync( Reservation reservation );
    void Delete( Reservation reservation );
    Task<int> GetOverlappingReservationsCountAsync( Guid roomTypeId, DateOnly arrival, DateOnly departure );
    Task<IEnumerable<Reservation>> GetFilteredAsync( ReservationFilter filter );
    Task<bool> HasReservationsAsync( Guid roomTypeId );
}