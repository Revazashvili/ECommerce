using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Models;
using Services.Common.Domain;

namespace Products.Infrastructure.Repositories;

public class ProductRepository(ProductsContext context) : IProductRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task<List<Product>> GetAsync(CancellationToken cancellationToken)
    {
        return context.Products
            .Include(product => product.Categories)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Products.FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

    public Task<List<Product>> SearchAsync(string? name, List<int>? categories, CancellationToken cancellationToken)
    {
        IQueryable<Product> productsQueryable = null;
        
        if(categories is not null && categories.Any())
            productsQueryable = context.Products
                .Include(product => product.Categories.Where(category => categories.Contains(category.Id)))
                .AsNoTracking();
        else
            productsQueryable = context.Products.Include(product => product.Categories).AsNoTracking();
        
        if (!string.IsNullOrEmpty(name))
            productsQueryable = productsQueryable.Where(product => product.Name.Contains(name));

        return productsQueryable.ToListAsync(cancellationToken);
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken)
    {
        var entityEntry = await context.Products.AddAsync(product,cancellationToken);
        return entityEntry.Entity;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken) =>
        context.Products.AnyAsync(product => product.Id == id, cancellationToken);
}