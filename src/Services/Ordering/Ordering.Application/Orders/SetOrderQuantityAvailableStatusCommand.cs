using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Exceptions;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record SetOrderQuantityAvailableStatusCommand(Guid OrderNumber) : IValidatedCommand<None>;

public class SetOrderQuantityAvailableStatusCommandHandler : IValidatedCommandHandler<SetOrderQuantityNotAvailableStatusCommand,None>
{
    private readonly ILogger<SetOrderQuantityAvailableStatusCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public SetOrderQuantityAvailableStatusCommandHandler(ILogger<SetOrderQuantityAvailableStatusCommandHandler> logger,
        IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task<Either<None, ValidationResult>> Handle(SetOrderQuantityNotAvailableStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber, cancellationToken);

            if (order is null)
                throw new OrderingException($"can't find order with OrderNumber: {request.OrderNumber}");

            order.SetAvailableQuantityStatus();
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