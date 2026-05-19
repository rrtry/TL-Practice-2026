using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IPropertyService
{
    Task<IEnumerable<Property>> GetAllPropertiesAsync();
    Task<Property?> GetPropertyByIdAsync( Guid id );
    Task<Property> CreatePropertyAsync( Property property );
    Task UpdatePropertyAsync( Property property );
    Task DeletePropertyAsync( Guid id );
};
