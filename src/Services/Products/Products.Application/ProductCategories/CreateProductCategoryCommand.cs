using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.ProductCategories;

public record CreateProductCategoryCommand(string Name) : IValidatedCommand<ProductCategory>;

public class CreateProductCategoryCommandHandler(ILogger<CreateProductCategoryCommandHandler> logger,
        IProductCategoryRepository productCategoryRepository)
    : IValidatedCommandHandler<CreateProductCategoryCommand,ProductCategory>
{
    public async Task<Either<ProductCategory, ValidationResult>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = new ProductCategory(request.Name);
            var result = await productCategoryRepository.AddAsync(productCategory, cancellationToken);

            await productCategoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(CreateProductCategoryCommandHandler));
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