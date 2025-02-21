using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.GetProducts;

public record GetProductsQuery : IValidatedQuery<IEnumerable<Product>>;