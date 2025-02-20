using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.SetOrderQuantityNotAvailableStatus;

public class SetOrderQuantityNotAvailableStatusCommandHandler(
    ILogger<SetOrderQuantityNotAvailableStatusCommandHandler> logger,
    IOrderRepository orderRepository)
    : IValidatedCommandHandler<SetOrderQuantityNotAvailableStatusCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(SetOrderQuantityNotAvailableStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"can't find order with OrderNumber: {request.OrderNumber}");

            order.SetUnAvailableQuantityStatus();
            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return None.Instance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}",
                nameof(SetOrderQuantityNotAvailableStatusCommand));
            return new ValidationResult("can't change order status");
        }
    }
}