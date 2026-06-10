namespace Domain;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
