using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.ProductCategories;

public record CreateProductCategoryCommand(string Name) : IValidatedCommand<ProductCategory>;

public class CreateProductCategoryCommandHandler : IValidatedCommandHandler<CreateProductCategoryCommand,ProductCategory>
{
    private readonly ILogger<CreateProductCategoryCommandHandler> _logger;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public CreateProductCategoryCommandHandler(ILogger<CreateProductCategoryCommandHandler> logger, IProductCategoryRepository productCategoryRepository)
    {
        _logger = logger;
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task<Either<ProductCategory, ValidationResult>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = new ProductCategory(request.Name);
            var result = await _productCategoryRepository.AddAsync(productCategory, cancellationToken);

            await _productCategoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(CreateProductCategoryCommandHandler));
            return new ValidationResult("Can't create product category");
        }
    }
}

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