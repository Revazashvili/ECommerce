using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.GetProductCategories;

public record GetProductCategoriesQuery : IValidatedQuery<IEnumerable<ProductCategory>>;