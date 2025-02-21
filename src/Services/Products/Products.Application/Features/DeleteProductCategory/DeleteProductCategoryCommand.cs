using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Products.Application.Features.DeleteProductCategory;

public record DeleteProductCategoryCommand(int Id) : IValidatedCommand<None>;