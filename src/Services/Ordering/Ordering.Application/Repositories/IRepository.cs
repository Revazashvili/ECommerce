namespace Ordering.Application.Repositories;

public interface IRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }
}