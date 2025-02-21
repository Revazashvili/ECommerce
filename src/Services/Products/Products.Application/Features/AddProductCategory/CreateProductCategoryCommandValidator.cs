using FluentValidation;

namespace Products.Application.Features.AddProductCategory;

public class CreateProductCategoryCommandValidator : AbstractValidator<CreateProductCategoryCommand>
{
    public CreateProductCategoryCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
    }
}