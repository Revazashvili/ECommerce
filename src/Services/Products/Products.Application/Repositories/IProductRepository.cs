using Products.Domain.Entities;
using Services.Common.Domain;

namespace Products.Application.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Product>> SearchAsync(string? name, List<int>? categories,CancellationToken cancellationToken);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}