using Products.Domain.Entities;

namespace Products.Application.Repositories;

public interface IProductCategoryRepository : IRepository<ProductCategory>
{
    Task<List<ProductCategory>> GetAsync(int[] ids,CancellationToken cancellationToken);
    Task<List<ProductCategory>> GetAsync(CancellationToken cancellationToken);
    Task<ProductCategory> AddAsync(ProductCategory productCategory,CancellationToken cancellationToken);
    ProductCategory Update(ProductCategory productCategory);
    ProductCategory Remove(ProductCategory productCategory);
}