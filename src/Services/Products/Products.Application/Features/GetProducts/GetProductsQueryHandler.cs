using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Application.Features.AddProduct;
using Products.Application.Products;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Features.GetProducts;

public class GetProductsQueryHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository)
    : IValidatedQueryHandler<GetProductsQuery,IEnumerable<Product>>
{
    public async Task<Either<IEnumerable<Product>, ValidationResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productRepository.GetAsync(cancellationToken);
            return products;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}", nameof(GetProductsQueryHandler));
            return new ValidationResult("Can't return product list");
        }
    }
}