using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.ProductCategories;

public record DeleteProductCategoryCommand(int Id) : IValidatedCommand<None>;

public class DeleteProductCategoryCommandHandler : IValidatedCommandHandler<DeleteProductCategoryCommand,None>
{
    
    private readonly ILogger<CreateProductCategoryCommandHandler> _logger;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public DeleteProductCategoryCommandHandler(ILogger<CreateProductCategoryCommandHandler> logger, IProductCategoryRepository productCategoryRepository)
    {
        _logger = logger;
        _productCategoryRepository = productCategoryRepository;
    }

    
    public async Task<Either<None, ValidationResult>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = new ProductCategory(request.Id);

            _productCategoryRepository.Remove(productCategory);
            await _productCategoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return None.Instance;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler} while deleting product category Id: {Id}",
                nameof(CreateProductCategoryCommandHandler), request.Id);
            return new ValidationResult("Can't delete product category");
        }
    }
}

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