using Domain;

namespace HotelManagement.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync( CancellationToken cancellationToken = default )
        => Task.FromResult( 0 );
}
