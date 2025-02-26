using FluentValidation;

namespace Ordering.Application.Features.GetOrders;

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
        
        RuleFor(command => command.PageNumber)
            .NotNull()
            .WithMessage("PageNumber must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageNumber must be greater or equal to One.");
        
        RuleFor(command => command.PageSize)
            .NotNull()
            .WithMessage("PageSize must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize must be greater or equal to One.");
    }
}