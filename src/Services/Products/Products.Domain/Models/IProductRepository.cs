using Products.Domain.Entities;

namespace Products.Domain.Models;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
}