using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record SetPendingStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;

public class SetPendingStatusCommandHandler : IValidatedCommandHandler<SetPendingStatusCommand,None>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public SetPendingStatusCommandHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    public async Task<Either<None, ValidationResult>> Handle(SetPendingStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber,cancellationToken);
            
            if(order is null)
                return new ValidationResult("Can't find order");

            order.SetPendingStatus();
            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return None.Instance;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(SetPendingStatusCommandHandler));
            return new ValidationResult("Can't change order status");
        }
    }
}