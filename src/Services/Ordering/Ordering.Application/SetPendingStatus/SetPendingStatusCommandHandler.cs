using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Models;

namespace Ordering.Application.SetPendingStatus;

public class SetPendingStatusCommandHandler(ILogger<SetPendingStatusCommandHandler> logger, IOrderRepository orderRepository)
    : IValidatedCommandHandler<SetPendingStatusCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(SetPendingStatusCommand request, CancellationToken cancellationToken)
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
            logger.LogError(exception,"Error occured in {Handler}",nameof(SetPendingStatusCommandHandler));
            return new ValidationResult("Can't change order status");
        }
    }
}