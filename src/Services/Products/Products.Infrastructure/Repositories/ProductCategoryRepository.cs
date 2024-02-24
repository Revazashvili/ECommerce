using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Models;
using Services.Common.Domain;

namespace Products.Infrastructure.Repositories;

public class ProductCategoryRepository(ProductsContext context) : IProductCategoryRepository
{
    public IUnitOfWork UnitOfWork => context;

    public Task<List<ProductCategory>> GetAsync(int[] ids, CancellationToken cancellationToken) => 
        context.ProductCategories
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken);

    public Task<List<ProductCategory>> GetAsync(CancellationToken cancellationToken) => 
        context.ProductCategories.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<ProductCategory> AddAsync(ProductCategory productCategory,CancellationToken cancellationToken)
    {
        var entityEntry = await context.ProductCategories.AddAsync(productCategory,cancellationToken);
        return entityEntry.Entity;
    }

    public ProductCategory Update(ProductCategory productCategory) =>
        context.ProductCategories.Update(productCategory).Entity;

    public ProductCategory Remove(ProductCategory productCategory) =>
        context.ProductCategories.Remove(productCategory).Entity;
}