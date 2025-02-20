using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Features.GetOrderById;

public class GetOrderByOrderNumberQueryHandler(ILogger<GetOrderByOrderNumberQueryHandler> logger,
    IOrderRepository orderRepository)
    : IValidatedQueryHandler<GetOrderByOrderNumberQuery, Order>
{
    public async Task<Either<Order, ValidationResult>> Handle(GetOrderByOrderNumberQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await orderRepository.GetByOrderNumberAsync(request.OrderNumber,cancellationToken);
            
            if(order is null)
                return new ValidationResult("Can't find order");

            return order;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(GetOrderByOrderNumberQueryHandler));
            return new ValidationResult("Can't retrieve order");
        }
    }
}