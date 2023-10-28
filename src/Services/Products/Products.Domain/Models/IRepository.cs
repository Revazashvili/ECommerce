namespace Products.Domain.Models;

public interface IRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }
}