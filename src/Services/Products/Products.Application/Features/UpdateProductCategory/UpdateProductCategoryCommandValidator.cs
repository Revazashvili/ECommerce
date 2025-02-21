using FluentValidation;

namespace Products.Application.Features.UpdateProductCategory;

public class UpdateProductCategoryCommandValidator : AbstractValidator<UpdateProductCategoryCommand>
{
    public UpdateProductCategoryCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
    }
}