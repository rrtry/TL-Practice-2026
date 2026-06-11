using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _context;

    public PropertyRepository( AppDbContext context )
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetByCityAsync( string city )
    {
        return await _context.Properties
            .Where( p => p.City == city )
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Property?> GetByIdAsync( Guid id )
    {
        return await _context.Properties
            .AsNoTracking()
            .FirstOrDefaultAsync( p => p.Id == id );
    }

    public async Task<Property?> GetByIdForUpdateAsync( Guid id )
    {
        return await _context.Properties.FindAsync( id );
    }

    public async Task AddAsync( Property property )
    {
        await _context.Properties.AddAsync( property );
    }

    public Task UpdateAsync( Property property )
    {
        _context.Properties.Update( property );
        return Task.CompletedTask;
    }

    public async Task DeleteByIdAsync( Guid id )
    {
        var entity = await _context.Properties.FindAsync( id );
        if ( entity != null )
        {
            _context.Properties.Remove( entity );
        }
    }

    public void Delete( Property property )
    {
        _context.Properties.Remove( property );
    }

    public async Task<bool> ExistsAsync( Guid id )
    {
        return await _context.Properties.AnyAsync( p => p.Id == id );
    }
}