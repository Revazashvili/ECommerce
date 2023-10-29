using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;
using Services.Common;

namespace Ordering.Application.Orders;

public record GetOrdersQuery(DateTime From,DateTime To,Pagination Pagination) : IValidatedQuery<IEnumerable<Order>>;

public class GetOrdersQueryHandler : IValidatedQueryHandler<GetOrdersQuery, IEnumerable<Order>>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    public async Task<Either<IEnumerable<Order>, ValidationResult>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _orderRepository.GetOrdersAsync(request.From, request.To, request.Pagination,cancellationToken);

            return orders;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(GetOrdersQueryHandler));
            return new ValidationResult("Can't retrieve orders");
        }
    }
}