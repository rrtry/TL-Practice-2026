using Domain.Entities;

namespace Domain.Repositories;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<IEnumerable<Property>> GetByCityAsync( string city );
    Task<Property?> GetByIdForUpdateAsync( Guid id );
    Task<Property?> GetByIdAsync( Guid id );
    Task AddAsync( Property property );
    Task UpdateAsync( Property property );
    Task DeleteByIdAsync( Guid id );
    void Delete( Property property );
    Task<bool> ExistsAsync( Guid id );
}
