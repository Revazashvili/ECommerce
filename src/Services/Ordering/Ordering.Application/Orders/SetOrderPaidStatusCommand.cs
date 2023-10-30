using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record SetOrderPaidStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;

public class SetOrderPaidStatusCommandHandler : IValidatedCommandHandler<SetOrderPaidStatusCommand,None>
{
    private readonly ILogger<SetOrderPaidStatusCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public SetOrderPaidStatusCommandHandler(ILogger<SetOrderPaidStatusCommandHandler> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task<Either<None, ValidationResult>> Handle(SetOrderPaidStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"can't find order with OrderNumber: {request.OrderNumber}");

            order.SetPaidStatus();
            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return None.Instance;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}",
                nameof(SetOrderQuantityNotAvailableStatusCommandHandler));
            return new ValidationResult("can't change order status");
        }
    }
}