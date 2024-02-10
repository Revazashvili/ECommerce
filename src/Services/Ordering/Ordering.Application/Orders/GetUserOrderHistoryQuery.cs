using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Application.Services;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record GetUserOrderHistoryQuery : IValidatedQuery<IEnumerable<Order>>;

public class GetUserOrderHistoryQueryHandler : IValidatedQueryHandler<GetUserOrderHistoryQuery, IEnumerable<Order>>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IIdentityService _identityService;

    public GetUserOrderHistoryQueryHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository,
        IIdentityService identityService)
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _identityService = identityService;
    }
    
    public async Task<Either<IEnumerable<Order>, ValidationResult>> Handle(GetUserOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = _identityService.GetUserId();
            var orders = await _orderRepository.GetUserOrdersAsync(userId,cancellationToken);

            foreach (var order in orders)
            {
                order.SetCreatedStatus();
            }

            await _orderRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            
            return orders;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(GetOrdersQueryHandler));
            return new ValidationResult("Can't retrieve user orders");
        }
    }
}