using Contracts;
using Contracts.Mediatr.Wrappers;

namespace Products.Application.Features.CheckProductsQuantityAvailabilityCommand;

public record CheckProductsQuantityAvailabilityCommand(Guid OrderNumber, Dictionary<Guid, int> Products) : IValidatedCommand<None>;