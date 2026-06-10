using Domain;

namespace HotelManagement.Tests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync() => Task.FromResult( 0 );
}
