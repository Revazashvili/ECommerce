using FluentValidation;
using Products.Application.Repositories;

namespace Products.Application.Features.UpdateProductStock;

public class UpdateProductStockCommandValidator : AbstractValidator<UpdateProductStockCommand>
{
    public UpdateProductStockCommandValidator(IProductRepository productRepository)
    {
        RuleFor(command => command.Id)
            .NotNull()
            .WithMessage("Id must not be null.")
            .NotEmpty()
            .WithMessage("Id must be greater or equal to 1.")
            .MustAsync(async (id, cancellationToken) => await productRepository.ExistsAsync(id, cancellationToken))
            .WithMessage("Product with this id doesn't exists");
        
        RuleFor(command => command.Quantity)
            .NotNull()
            .WithMessage("Quantity must not be null.")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity must be greater or equal to 0.");
    }
}