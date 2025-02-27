using FluentValidation;
using Products.Application.Repositories;

namespace Products.Application.Features.AddProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductCategoryRepository productCategoryRepository)
    {
        RuleFor(command => command.Name)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
        
        RuleFor(command => command.ImageBase64)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
        
        RuleFor(command => command.Quantity)
            .NotNull()
            .WithMessage("Quantity must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity must be greater or equal to 1.");
        
        RuleFor(command => command.Price)
            .NotNull()
            .WithMessage("Price must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Price must be greater or equal to 1.");
        
        RuleFor(command => command.Categories)
            .NotNull()
            .WithMessage("Categories must not be null.")
            .NotEmpty()
            .WithMessage("Categories must not be empty.")
            .MustAsync(async (command, context, cancellationToken) =>
            {
                var categories = await productCategoryRepository.GetAsync(command.Categories.ToArray(), cancellationToken);
                return categories.Any();
            });
    }
}