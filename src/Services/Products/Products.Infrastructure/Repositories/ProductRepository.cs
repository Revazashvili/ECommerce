using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsContext _context;
    public ProductRepository(ProductsContext context) => _context = context;

    public IUnitOfWork UnitOfWork => _context;

    public Task<List<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return _context.Products
            .Include(product => product.Categories)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<List<Product>> SearchAsync(string? name, List<int>? categories, CancellationToken cancellationToken)
    {
        IQueryable<Product> productsQueryable = null;
        
        if(categories is not null && categories.Any())
            productsQueryable = _context.Products
                .Include(product => product.Categories.Where(category => categories.Contains(category.Id)))
                .AsNoTracking();
        else
            productsQueryable = _context.Products.Include(product => product.Categories).AsNoTracking();
        
        if (!string.IsNullOrEmpty(name))
            productsQueryable = productsQueryable.Where(product => product.Name.Contains(name));

        return productsQueryable.ToListAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        var entityEntry = await _context.Products.AddAsync(product,cancellationToken);
        return entityEntry.Entity;
    }
}