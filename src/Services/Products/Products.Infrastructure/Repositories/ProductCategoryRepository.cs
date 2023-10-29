using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Models;
using Services.Common.Domain;

namespace Products.Infrastructure.Repositories;

public class ProductCategoryRepository : IProductCategoryRepository
{
    private readonly ProductsContext _context;
    public ProductCategoryRepository(ProductsContext context) => _context = context;
    
    public IUnitOfWork UnitOfWork => _context;

    public Task<List<ProductCategory>> GetAsync(int[] ids, CancellationToken cancellationToken) => 
        _context.ProductCategories
            .Where(category => ids.Contains(category.Id))
            .ToListAsync(cancellationToken);

    public Task<List<ProductCategory>> GetAsync(CancellationToken cancellationToken) => 
        _context.ProductCategories.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<ProductCategory> AddAsync(ProductCategory productCategory,CancellationToken cancellationToken)
    {
        var entityEntry = await _context.ProductCategories.AddAsync(productCategory,cancellationToken);
        return entityEntry.Entity;
    }

    public ProductCategory Update(ProductCategory productCategory) =>
        _context.ProductCategories.Update(productCategory).Entity;

    public ProductCategory Remove(ProductCategory productCategory) =>
        _context.ProductCategories.Remove(productCategory).Entity;
}