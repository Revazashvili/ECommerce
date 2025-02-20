using FluentValidation;

namespace Ordering.Application.GetOrders;

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
        RuleFor(command => command.From)
            .NotNull()
            .WithMessage("From must not be null.")
            .GreaterThan(DateTime.MinValue)
            .WithMessage("From must be greater or equal to MinValue.");
        
        RuleFor(command => command.To)
            .NotNull()
            .WithMessage("To must not be null.")
            .GreaterThan(DateTime.MinValue)
            .WithMessage("To must be greater or equal to MinValue.");
        
        RuleFor(command => command.Pagination)
            .NotNull()
            .WithMessage("Pagination must not be null.");
    }
}