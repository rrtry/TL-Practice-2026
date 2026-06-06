using Domain.Entities;

namespace Domain.Repositories;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<IEnumerable<Property>> GetByCityAsync( string city );
    Task<Property?> GetByIdAsync( Guid id );
    Task AddAsync( Property property );
    Task UpdateAsync( Property property );
    Task DeleteAsync( Guid id );
    Task<bool> ExistsAsync( Guid id );
}
