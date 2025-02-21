using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.ProductCategories;

public record GetProductCategoriesQuery : IValidatedQuery<IEnumerable<ProductCategory>>;