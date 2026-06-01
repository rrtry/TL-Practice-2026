using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EfPropertyRepository : IPropertyRepository
{
    private readonly AppDbContext _context;

    public EfPropertyRepository( AppDbContext context )
    {
        _context = context;
    }

    public async Task<IEnumerable<Property>> GetAllAsync() =>
        await _context.Properties.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<Property>> GetByCityAsync( string city ) =>
        await _context.Properties
            .Where( p => p.City == city )
            .AsNoTracking()
            .ToListAsync();

    public async Task<Property?> GetByIdAsync( Guid id ) =>
        await _context.Properties.FindAsync( id );

    public async Task AddAsync( Property property )
    {
        property.Id = Guid.NewGuid();
        await _context.Properties.AddAsync( property );
    }

    public Task UpdateAsync( Property property )
    {
        _context.Properties.Update( property );
        return Task.CompletedTask;
    }

    public async Task DeleteAsync( Guid id )
    {
        var entity = await _context.Properties.FindAsync( id );
        if ( entity != null )
        {
            _context.Properties.Remove( entity );
        }
    }

    public async Task<bool> ExistsAsync( Guid id ) =>
        await _context.Properties.AnyAsync( p => p.Id == id );
}