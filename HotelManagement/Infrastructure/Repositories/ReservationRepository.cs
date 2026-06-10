using Domain.Entities;
using Domain.Filters;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository( AppDbContext context )
    {
        _context = context;
    }

    public async Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync( Guid id )
    {
        return await _context.Reservations
            .AsNoTracking()
            .FirstOrDefaultAsync( r => r.Id == id );
    }

    public async Task<Reservation?> GetByIdAsyncForUpdate( Guid id )
    {
        return await _context.Reservations.FindAsync( id );
    }

    public async Task AddAsync( Reservation reservation )
    {
        await _context.Reservations.AddAsync( reservation );
    }

    public void Delete( Reservation reservation )
    {
        _context.Reservations.Remove( reservation );
    }

    public async Task DeleteAsyncById( Guid id )
    {
        var entity = await _context.Reservations.FindAsync( id );
        if ( entity != null )
        {
            _context.Reservations.Remove( entity );
        }
    }

    public async Task<int> GetOverlappingReservationsCountAsync(
        Guid roomTypeId, DateOnly arrival, DateOnly departure )
    {
        return await _context.Reservations
            .CountAsync( r =>
                r.RoomTypeId == roomTypeId &&
                r.ArrivalDate < departure &&
                r.DepartureDate > arrival );
    }

    public async Task<IEnumerable<Reservation>> GetFilteredAsync( ReservationFilter filter )
    {
        var query = _context.Reservations.AsNoTracking().AsQueryable();

        if ( filter.PropertyId.HasValue )
        {
            query = query.Where( r => r.PropertyId == filter.PropertyId.Value );
        }

        if ( filter.FromDate.HasValue )
        {
            query = query.Where( r => r.ArrivalDate >= filter.FromDate.Value );
        }

        if ( filter.ToDate.HasValue )
        {
            query = query.Where( r => r.DepartureDate <= filter.ToDate.Value );
        }

        if ( !string.IsNullOrEmpty( filter.GuestName ) )
        {
            query = query.Where( r => r.GuestName.Contains( filter.GuestName ) );
        }

        return await query.ToListAsync();
    }
}