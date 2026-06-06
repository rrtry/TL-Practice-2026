using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfRoomTypeRepository : IRoomTypeRepository
{
    private readonly AppDbContext _context;

    public EfRoomTypeRepository( AppDbContext context )
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId ) =>
        await _context.RoomTypes
            .Where( rt => rt.PropertyId == propertyId )
            .AsNoTracking()
            .ToListAsync();

    public async Task<RoomType?> GetByIdAsync( Guid id ) =>
        await _context.RoomTypes.FindAsync( id );

    public async Task AddAsync( RoomType roomType )
    {
        roomType.Id = Guid.NewGuid();
        await _context.RoomTypes.AddAsync( roomType );
    }

    public Task UpdateAsync( RoomType roomType )
    {
        _context.RoomTypes.Update( roomType );
        return Task.CompletedTask;
    }

    public async Task DeleteAsync( Guid id )
    {
        var entity = await _context.RoomTypes.FindAsync( id );
        if ( entity != null )
        {
            _context.RoomTypes.Remove( entity );
        }
    }

    public async Task<bool> ExistsAsync( Guid id ) =>
        await _context.RoomTypes.AnyAsync( rt => rt.Id == id );

    public async Task<bool> HasReservationsAsync( Guid roomTypeId ) =>
        await _context.Reservations.AnyAsync( r => r.RoomTypeId == roomTypeId );
}