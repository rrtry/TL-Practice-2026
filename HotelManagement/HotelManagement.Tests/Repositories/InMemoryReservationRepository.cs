using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;

namespace HotelManagement.Tests.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly ConcurrentDictionary<Guid, Reservation> _reservations = new();

    public Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return Task.FromResult( _reservations.Values.AsEnumerable() );
    }

    public Task<Reservation?> GetByIdAsync( Guid id )
    {
        return GetByIdAsyncForUpdate( id );
    }

    public Task<Reservation?> GetByIdAsyncForUpdate( Guid id )
    {
        return Task.FromResult( _reservations.TryGetValue( id, out var res ) ? res : null );
    }

    public Task AddAsync( Reservation reservation )
    {
        reservation.Id = Guid.NewGuid();
        _reservations.TryAdd( reservation.Id, reservation );

        return Task.CompletedTask;
    }

    public Task DeleteAsyncById( Guid id )
    {
        _reservations.TryRemove( id, out _ );
        return Task.CompletedTask;
    }

    public void Delete( Reservation reservation )
    {
        _reservations.TryRemove( reservation.Id, out _ );
    }

    public Task<int> GetOverlappingReservationsCountAsync( Guid roomTypeId, DateOnly arrival, DateOnly departure )
    {
        var count = _reservations.Values.Count( r =>
            r.RoomTypeId == roomTypeId &&
            r.ArrivalDate < departure &&
            r.DepartureDate > arrival );

        return Task.FromResult( count );
    }

    public Task<IEnumerable<Reservation>> GetFilteredAsync( ReservationFilter filter )
    {
        var query = _reservations.Values.AsQueryable();
        var propertyId = filter.PropertyId;
        var fromDate = filter.FromDate;
        var toDate = filter.ToDate;
        var guestName = filter.GuestName;

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

    public Task<bool> HasReservationsAsync( Guid roomTypeId )
    {
        var exists = _reservations.Values.Any( r => r.RoomTypeId == roomTypeId );
        return Task.FromResult( exists );
    }
}