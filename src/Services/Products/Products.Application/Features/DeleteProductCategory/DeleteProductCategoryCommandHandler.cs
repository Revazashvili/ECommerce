using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Application.Features.AddProductCategory;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.ProductCategories;

public class DeleteProductCategoryCommandHandler(ILogger<CreateProductCategoryCommandHandler> logger,
    IProductCategoryRepository productCategoryRepository)
    : IValidatedCommandHandler<DeleteProductCategoryCommand,None>
{
    public async Task<Either<None, ValidationResult>> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = new ProductCategory(request.Id);

            productCategoryRepository.Remove(productCategory);
            await productCategoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return None.Instance;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler} while deleting product category Id: {Id}",
                nameof(CreateProductCategoryCommandHandler), request.Id);
            return new ValidationResult("Can't delete product category");
        }
    }
}