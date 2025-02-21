using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.AddProductCategory;

public record CreateProductCategoryCommand(string Name) : IValidatedCommand<ProductCategory>;