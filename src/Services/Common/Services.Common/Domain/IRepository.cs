namespace Services.Common.Domain;

public interface IRepository<T> where T : class
{
    IUnitOfWork UnitOfWork { get; }
}