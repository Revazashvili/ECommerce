using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record SetOrderPaidStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;

public class SetOrderPaidStatusCommandHandler(ILogger<SetOrderPaidStatusCommandHandler> logger,
        IOrderRepository orderRepository)
    : IValidatedCommandHandler<SetOrderPaidStatusCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(SetOrderPaidStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"can't find order with OrderNumber: {request.OrderNumber}");

            order.SetPaidStatus();
            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return None.Instance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(SetOrderQuantityNotAvailableStatusCommandHandler));
            return new ValidationResult("can't change order status");
        }
    }
}