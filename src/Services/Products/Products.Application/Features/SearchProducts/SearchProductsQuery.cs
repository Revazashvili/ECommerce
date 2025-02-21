using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.SearchProducts;

public record SearchProductsQuery(string Name,List<int> Categories) : IValidatedQuery<IEnumerable<Product>>;