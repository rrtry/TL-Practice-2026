using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IReservationRepository
{
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task<Reservation?> GetByIdAsync( Guid id );
    Task AddAsync( Reservation reservation );
    Task DeleteAsync( Guid id );
    Task<int> GetOverlappingReservationsCountAsync( Guid roomTypeId, DateTime arrival, DateTime departure );
    Task<IEnumerable<Reservation>> GetFilteredAsync( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName );
}