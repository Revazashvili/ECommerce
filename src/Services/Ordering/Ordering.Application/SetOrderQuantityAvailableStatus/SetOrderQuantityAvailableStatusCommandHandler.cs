using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.SetOrderQuantityAvailableStatus;

public class SetOrderQuantityAvailableStatusCommandHandler(
    ILogger<SetOrderQuantityAvailableStatusCommandHandler> logger,
    IOrderRepository orderRepository)
    : IValidatedCommandHandler<SetOrderQuantityAvailableStatusCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(SetOrderQuantityAvailableStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"can't find order with OrderNumber: {request.OrderNumber}");

            order.SetAvailableQuantityStatus();
            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return None.Instance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(SetOrderQuantityAvailableStatusCommand));
            return new ValidationResult("can't change order status");
        }
    }
}