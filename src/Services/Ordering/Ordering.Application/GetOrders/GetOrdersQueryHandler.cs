using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.GetOrders;

public class GetOrdersQueryHandler(ILogger<GetOrdersQueryHandler> logger, IOrderRepository orderRepository)
    : IValidatedQueryHandler<GetOrdersQuery, IEnumerable<Order>>
{
    public async Task<Either<IEnumerable<Order>, ValidationResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await orderRepository.GetOrdersAsync(request.From, request.To, request.Pagination,cancellationToken);

            return orders;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(GetOrdersQueryHandler));
            return new ValidationResult("Can't retrieve orders");
        }
    }
}