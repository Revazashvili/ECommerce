using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Products;

public record UpdateProductStockCommand(Guid Id, int Quantity) : IValidatedCommand<Product>;

public class UpdateProductStockCommandHandler : IValidatedCommandHandler<UpdateProductStockCommand, Product>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public UpdateProductStockCommandHandler(ILogger<CreateProductCommandHandler> logger,IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    
    public async Task<Either<Product, ValidationResult>> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id,cancellationToken);
            if (product is null)
                return new ValidationResult("Can't find product");
            
            product.UpdateQuantity(request.Quantity);
            await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return product;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler} with Id: {Id}",
                nameof(UpdateProductStockCommandHandler), request.Id);
            return new ValidationResult("Can't update product stock");
        }
    }
}

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
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity must be greater or equal to 1.");
    }
}