using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Application.Repositories;

namespace Ordering.Application.Features.SetOrderPendingStatus;

public class SetOrderPendingStatusCommandHandler(ILogger<SetOrderPendingStatusCommandHandler> logger, IOrderRepository orderRepository)
    : IValidatedCommandHandler<SetOrderPendingStatusCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(SetOrderPendingStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber,cancellationToken);
            
            if(order is null)
                return new ValidationResult("Can't find order");

            order.SetPendingStatus();
            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return None.Instance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(SetOrderPendingStatusCommandHandler));
            return new ValidationResult("Can't change order status");
        }
    }
}