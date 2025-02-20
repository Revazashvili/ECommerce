using FluentValidation;

namespace Ordering.Application.GetOrderById;

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