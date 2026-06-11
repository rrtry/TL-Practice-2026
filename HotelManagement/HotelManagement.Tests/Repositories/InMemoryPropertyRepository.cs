using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Repositories;

namespace HotelManagement.Tests.Repositories;

public class InMemoryPropertyRepository : IPropertyRepository
{
    private readonly ConcurrentDictionary<Guid, Property> _properties = new();

    public Task<IEnumerable<Property>> GetAllAsync()
    {
        return Task.FromResult( _properties.Values.AsEnumerable() );
    }

    public Task<Property?> GetByIdAsync( Guid id )
    {
        return Task.FromResult( _properties.TryGetValue( id, out var prop ) ? prop : null );
    }

    public Task<Property?> GetByIdForUpdateAsync( Guid id )
    {
        return Task.FromResult( _properties.TryGetValue( id, out var prop ) ? prop : null );
    }

    public Task<IEnumerable<Property>> GetByCityAsync( string city )
    {
        return Task.FromResult( _properties.Values.Where( p =>
            p.City.Equals( city, StringComparison.OrdinalIgnoreCase ) )
        );
    }

    public Task AddAsync( Property property )
    {
        property.Id = Guid.NewGuid();
        _properties.TryAdd( property.Id, property );
        return Task.CompletedTask;
    }

    public Task UpdateAsync( Property property )
    {
        _properties[ property.Id ] = property;
        return Task.CompletedTask;
    }

    public void Delete( Property property )
    {
        _properties.TryRemove( property.Id, out _ );
    }

    public Task<bool> ExistsAsync( Guid id )
    {
        return Task.FromResult( _properties.ContainsKey( id ) );
    }
}
