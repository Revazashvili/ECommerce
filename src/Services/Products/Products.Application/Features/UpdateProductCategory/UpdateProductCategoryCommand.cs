using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.UpdateProductCategory;

public record UpdateProductCategoryCommand(int Id,string Name) : IValidatedCommand<ProductCategory>;