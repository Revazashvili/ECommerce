using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;

namespace Ordering.Application.Orders;

public record GetOrderByOrderNumberQuery(Guid OrderNumber) : IValidatedQuery<Order>;

public class GetOrderByOrderNumberQueryHandler(ILogger<CancelOrderCommandHandler> logger,
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

public class GetOrderByOrderNumberQueryValidator : AbstractValidator<GetOrderByOrderNumberQuery>
{
    public GetOrderByOrderNumberQueryValidator()
    {
        RuleFor(command => command.OrderNumber)
            .NotNull()
            .WithMessage("OrderNumber must not be null.")
            .NotEmpty()
            .WithMessage("OrderNumber must not be empty.");
    }
}