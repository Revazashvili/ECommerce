using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsContext _context;
    public ProductRepository(ProductsContext context) => _context = context;

    public IUnitOfWork UnitOfWork => _context;
    
    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        var entityEntry = await _context.Products.AddAsync(product,cancellationToken);
        return entityEntry.Entity;
    }
}