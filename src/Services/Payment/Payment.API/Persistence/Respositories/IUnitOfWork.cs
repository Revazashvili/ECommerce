namespace Payment.API.Persistence.Respositories;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    void RejectChanges();
}