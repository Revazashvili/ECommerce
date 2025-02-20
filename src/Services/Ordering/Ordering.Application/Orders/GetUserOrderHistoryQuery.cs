using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record GetUserOrderHistoryQuery : IValidatedQuery<IEnumerable<Order>>;

public class GetUserOrderHistoryQueryHandler(ILogger<GetUserOrderHistoryQueryHandler> logger,
        IOrderRepository orderRepository,
        IIdentityService identityService)
    : IValidatedQueryHandler<GetUserOrderHistoryQuery, IEnumerable<Order>>
{
    public async Task<Either<IEnumerable<Order>, ValidationResult>> Handle(GetUserOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = identityService.GetUserId();
            var orders = await orderRepository.GetUserOrdersAsync(userId,cancellationToken);

            foreach (var order in orders)
            {
                order.SetCreatedStatus();
            }

            await orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return orders;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(GetUserOrderHistoryQueryHandler));
            return new ValidationResult("Can't retrieve user orders");
        }
    }
}