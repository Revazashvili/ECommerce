using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Products.Application.ProductCategories;

public record DeleteProductCategoryCommand(int Id) : IValidatedCommand<None>;