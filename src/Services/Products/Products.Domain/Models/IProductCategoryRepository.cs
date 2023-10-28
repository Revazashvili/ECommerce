using Products.Domain.Entities;

namespace Products.Domain.Models;

public interface IProductCategoryRepository : IRepository<ProductCategory>
{
    Task<List<ProductCategory>> GetAsync(CancellationToken cancellationToken);
    Task<ProductCategory> AddAsync(ProductCategory productCategory,CancellationToken cancellationToken);
    ProductCategory Update(ProductCategory productCategory);
    ProductCategory Remove(ProductCategory productCategory);
}