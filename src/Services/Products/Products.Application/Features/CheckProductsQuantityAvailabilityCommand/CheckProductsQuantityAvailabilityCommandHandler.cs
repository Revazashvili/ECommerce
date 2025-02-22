using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using EventBridge.Dispatcher;
using Products.Domain.Models;

namespace Products.Application.Features.CheckProductsQuantityAvailabilityCommand;

public class CheckProductsQuantityAvailabilityCommandHandler(IProductRepository productRepository, IIntegrationEventDispatcher dispatcher)
    : IValidatedCommandHandler<CheckProductsQuantityAvailabilityCommand, None>
{
    public async Task<Either<None, ValidationResult>> Handle(CheckProductsQuantityAvailabilityCommand request, CancellationToken cancellationToken)
    {
        var productQuantityMapping = new Dictionary<Guid, bool>();
        foreach (var p in request.Products)
        {
            var product = await productRepository.GetByIdAsync(p.Key, cancellationToken);
            var hasEnoughQuantity = product is not null && product.Quantity >= p.Value;

            productQuantityMapping.Add(p.Key, hasEnoughQuantity);
        }

        if (productQuantityMapping.Any(pair => !pair.Value))
            await dispatcher.DispatchAsync("OrderQuantityNotAvailable", new OrderQuantityNotAvailableIntegrationEvent(request.OrderNumber), cancellationToken);
        else
            await dispatcher.DispatchAsync("OrderQuantityAvailable", new OrderQuantityAvailableIntegrationEvent(request.OrderNumber), cancellationToken);

        await productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return None.Instance;
    }
}