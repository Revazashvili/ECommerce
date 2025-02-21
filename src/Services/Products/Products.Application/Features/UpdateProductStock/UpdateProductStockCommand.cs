using Contracts.Mediatr.Wrappers;
using Products.Domain.Entities;

namespace Products.Application.Features.UpdateProductStock;

public record UpdateProductStockCommand(Guid Id, int Quantity) : IValidatedCommand<Product>;