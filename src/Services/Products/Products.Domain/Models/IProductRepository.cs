using Products.Domain.Entities;

namespace Products.Domain.Models;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAsync(CancellationToken cancellationToken);
    Task<List<Product>> SearchAsync(string? name, List<int>? categories,CancellationToken cancellationToken);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
}