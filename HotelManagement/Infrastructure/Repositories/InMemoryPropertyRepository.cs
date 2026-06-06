using System.Collections.Concurrent;
using Domain.Entities;
using Domain.Repositories;

namespace Infrastructure.Repositories;

public class InMemoryPropertyRepository : IPropertyRepository
{
    private readonly ConcurrentDictionary<Guid, Property> _properties = new();

    public Task<IEnumerable<Property>> GetAllAsync() =>
        Task.FromResult( _properties.Values.AsEnumerable() );

    public Task<Property?> GetByIdAsync( Guid id ) =>
        Task.FromResult( _properties.TryGetValue( id, out var prop ) ? prop : null );

    public Task<IEnumerable<Property>> GetByCityAsync( string city ) =>
        Task.FromResult( _properties.Values.Where( p =>
            p.City.Equals( city, StringComparison.OrdinalIgnoreCase ) )
        );

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

    public Task DeleteAsync( Guid id )
    {
        _properties.TryRemove( id, out _ );
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync( Guid id ) =>
        Task.FromResult( _properties.ContainsKey( id ) );
}
