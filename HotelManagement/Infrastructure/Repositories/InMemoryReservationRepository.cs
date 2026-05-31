using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly ConcurrentDictionary<Guid, Reservation> _reservations = new();

    public Task<IEnumerable<Reservation>> GetAllAsync() =>
        Task.FromResult( _reservations.Values.AsEnumerable() );

    public Task<Reservation?> GetByIdAsync( Guid id ) =>
        Task.FromResult( _reservations.TryGetValue( id, out var res ) ? res : null );

    public Task AddAsync( Reservation reservation )
    {
        reservation.Id = Guid.NewGuid();
        _reservations.TryAdd( reservation.Id, reservation );

        return Task.CompletedTask;
    }

    public Task DeleteAsync( Guid id )
    {
        _reservations.TryRemove( id, out _ );
        return Task.CompletedTask;
    }

    public Task<int> GetOverlappingReservationsCountAsync( Guid roomTypeId, DateTime arrival, DateTime departure )
    {
        var count = _reservations.Values.Count( r =>
            r.RoomTypeId == roomTypeId &&
            r.ArrivalDate < departure &&
            r.DepartureDate > arrival );

        return Task.FromResult( count );
    }

    public Task<IEnumerable<Reservation>> GetFilteredAsync( Guid? propertyId, DateTime? fromDate, DateTime? toDate, string? guestName )
    {
        var query = _reservations.Values.AsQueryable();
        if ( propertyId.HasValue )
        {
            query = query.Where( r => r.PropertyId == propertyId.Value );
        }

        if ( fromDate.HasValue )
        {
            query = query.Where( r => r.ArrivalDate >= fromDate.Value );
        }

        if ( toDate.HasValue )
        {
            query = query.Where( r => r.DepartureDate <= toDate.Value );
        }

        if ( !string.IsNullOrEmpty( guestName ) )
        {
            query = query.Where( r => r.GuestName.Contains( guestName, StringComparison.OrdinalIgnoreCase ) );
        }

        return Task.FromResult( query.AsEnumerable() );
    }
}