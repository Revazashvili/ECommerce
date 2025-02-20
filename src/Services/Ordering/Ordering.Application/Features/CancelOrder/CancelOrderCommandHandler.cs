using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.CancelOrder;

public class CancelOrderCommandHandler(ILogger<CancelOrderCommandHandler> logger, IOrderRepository orderRepository)
    : IValidatedCommandHandler<CancelOrderCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber,cancellationToken);
            
            if(order is null)
                return new ValidationResult("Can't find order");

            if (order.OrderStatus == OrderStatus.Cancelled)
                return None.Instance;
            
            order.Cancel();

            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return None.Instance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(CancelOrderCommandHandler));
            return new ValidationResult("Can't cancel order");
        }
    }
}