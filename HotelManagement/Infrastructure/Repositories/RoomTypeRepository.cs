using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoomTypeRepository : IRoomTypeRepository
{
    private readonly AppDbContext _context;

    public RoomTypeRepository( AppDbContext context )
    {
        _context = context;
    }

    public async Task<IEnumerable<RoomType>> GetByPropertyIdAsync( Guid propertyId )
    {
        return await _context.RoomTypes
            .Where( rt => rt.PropertyId == propertyId )
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<RoomType?> GetByIdAsyncForUpdate( Guid id )
    {
        return await _context.RoomTypes.FindAsync( id );
    }

    public async Task<RoomType?> GetByIdAsync( Guid id )
    {
        return await _context.RoomTypes
            .AsNoTracking()
            .FirstOrDefaultAsync( rt => rt.Id == id );
    }

    public async Task AddAsync( RoomType roomType )
    {
        await _context.RoomTypes.AddAsync( roomType );
    }

    public Task UpdateAsync( RoomType roomType )
    {
        _context.RoomTypes.Update( roomType );
        return Task.CompletedTask;
    }

    public void Delete( RoomType roomType )
    {
        _context.RoomTypes.Remove( roomType );
    }

    public async Task<bool> ExistsAsync( Guid id )
    {
        return await _context.RoomTypes.AnyAsync( rt => rt.Id == id );
    }

    public async Task<bool> HasRoomTypesForPropertyAsync( Guid propertyId )
    {
        return await _context.RoomTypes.AnyAsync( rt => rt.PropertyId == propertyId );
    }
}