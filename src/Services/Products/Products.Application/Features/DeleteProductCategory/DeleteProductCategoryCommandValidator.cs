using FluentValidation;
using Products.Application.ProductCategories;

namespace Products.Application.Features.DeleteProductCategory;

public class DeleteProductCategoryCommandValidator : AbstractValidator<DeleteProductCategoryCommand>
{
    public DeleteProductCategoryCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotNull()
            .WithMessage("Id must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Id must equal or more than 1.");
    }
}