using Domain;

namespace Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly HotelManagementDbContext _context;

    public UnitOfWork( HotelManagementDbContext context ) => _context = context;

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
