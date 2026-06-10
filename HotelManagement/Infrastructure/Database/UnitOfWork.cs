using Domain;

namespace Infrastructure.Database;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork( AppDbContext context ) => _context = context;

    public Task SaveChangesAsync() => _context.SaveChangesAsync();
}
