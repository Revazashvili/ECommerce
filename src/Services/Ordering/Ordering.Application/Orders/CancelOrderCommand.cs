using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record CancelOrderCommand(Guid OrderNumber) : IValidatedCommand<None>;

public class CancelOrderCommandHandler : IValidatedCommandHandler<CancelOrderCommand,None>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public CancelOrderCommandHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    public async Task<Either<None, ValidationResult>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber,cancellationToken);
            
            if(order is null)
                return new ValidationResult("Can't find order");

            if (order.OrderStatus == OrderStatus.Cancelled)
                return None.Instance;
            
            order.Cancel();

            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return None.Instance;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(CancelOrderCommandHandler));
            return new ValidationResult("Can't cancel order");
        }
    }
}