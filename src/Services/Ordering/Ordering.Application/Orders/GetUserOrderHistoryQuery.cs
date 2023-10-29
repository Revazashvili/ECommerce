using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record GetUserOrderHistoryQuery(int UserId) : IValidatedQuery<IEnumerable<Order>>;

public class GetUserOrderHistoryQueryHandler : IValidatedQueryHandler<GetUserOrderHistoryQuery, IEnumerable<Order>>
{
    private readonly ILogger<CancelOrderCommandHandler> _logger;
    private readonly IOrderRepository _orderRepository;

    public GetUserOrderHistoryQueryHandler(ILogger<CancelOrderCommandHandler> logger,IOrderRepository orderRepository)
    {
        _logger = logger;
        _orderRepository = orderRepository;
    }
    
    public async Task<Either<IEnumerable<Order>, ValidationResult>> Handle(GetUserOrderHistoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _orderRepository.GetUserOrdersAsync(request.UserId,cancellationToken);
            return orders;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(GetOrdersQueryHandler));
            return new ValidationResult("Can't retrieve user orders");
        }
    }
}

public class GetUserOrderHistoryQueryValidator : AbstractValidator<GetUserOrderHistoryQuery>
{
    public GetUserOrderHistoryQueryValidator()
    {
        RuleFor(command => command.UserId)
            .NotNull()
            .WithMessage("UserId must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("UserId must be greater or equal to 1.");
    }
}