using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.ProductCategories;

public record UpdateProductCategoryCommand(int Id,string Name) : IValidatedCommand<ProductCategory>;

public class UpdateProductCategoryCommandHandler : IValidatedCommandHandler<UpdateProductCategoryCommand,ProductCategory>
{
    private readonly ILogger<UpdateProductCategoryCommandHandler> _logger;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public UpdateProductCategoryCommandHandler(ILogger<UpdateProductCategoryCommandHandler> logger, IProductCategoryRepository productCategoryRepository)
    {
        _logger = logger;
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task<Either<ProductCategory, ValidationResult>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = new ProductCategory(request.Id,request.Name);
            var result = _productCategoryRepository.Update(productCategory);

            await _productCategoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {Handler}",nameof(UpdateProductCategoryCommandHandler));
            return new ValidationResult("Can't update product category");
        }
    }
}

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