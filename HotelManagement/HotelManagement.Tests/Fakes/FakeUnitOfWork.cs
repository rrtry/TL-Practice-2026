using Domain.Interfaces;

namespace HotelManagement.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task<int> SaveChangesAsync( CancellationToken cancellationToken = default )
        => Task.FromResult( 0 );
}
