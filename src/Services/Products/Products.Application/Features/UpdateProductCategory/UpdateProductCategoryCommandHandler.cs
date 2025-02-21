using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Features.UpdateProductCategory;

public class UpdateProductCategoryCommandHandler(ILogger<UpdateProductCategoryCommandHandler> logger,
    IProductCategoryRepository productCategoryRepository)
    : IValidatedCommandHandler<UpdateProductCategoryCommand,ProductCategory>
{
    public async Task<Either<ProductCategory, ValidationResult>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var productCategory = new ProductCategory(request.Id,request.Name);
            var result = productCategoryRepository.Update(productCategory);

            await productCategoryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(UpdateProductCategoryCommandHandler));
            return new ValidationResult("Can't update product category");
        }
    }
}