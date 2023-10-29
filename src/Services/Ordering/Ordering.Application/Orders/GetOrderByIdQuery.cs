using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record GetOrderByOrderNumberQuery(Guid OrderNumber) : IValidatedQuery<Order>;

public class GetOrderByOrderNumberQueryHandler : IValidatedQueryHandler<GetOrderByOrderNumberQuery, Order>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public GetOrderByOrderNumberQueryHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task<Either<Order, ValidationResult>> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByOrderNumberAsync(request.OrderNumber,cancellationToken);
            
            if(order is null)
                return new ValidationResult("Can't find order");

            return order;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(GetOrderByOrderNumberQueryHandler));
            return new ValidationResult("Can't retrieve order");
        }
    }
}