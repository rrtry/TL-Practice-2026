using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPropertyRepository
{
    Task<IEnumerable<Property>> GetAllAsync();
    Task<Property?> GetByIdAsync( Guid id );
    Task AddAsync( Property property );
    Task UpdateAsync( Property property );
    Task DeleteAsync( Guid id );
    Task<bool> ExistsAsync( Guid id );
}
