using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Application.Features.AddProduct;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Features.UpdateProductStock;

public class UpdateProductStockCommandHandler(ILogger<CreateProductCommandHandler> logger,
    IProductRepository productRepository)
    : IValidatedCommandHandler<UpdateProductStockCommand, Product>
{
    public async Task<Either<Product, ValidationResult>> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.GetByIdAsync(request.Id,cancellationToken);
            if (product is null)
                return new ValidationResult("Can't find product");

            if (product.Quantity == request.Quantity)
                return product;
            
            product.UpdateQuantity(request.Quantity);
            await productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return product;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler} with Id: {Id}",
                nameof(UpdateProductStockCommandHandler), request.Id);
            return new ValidationResult("Can't update product stock");
        }
    }
}