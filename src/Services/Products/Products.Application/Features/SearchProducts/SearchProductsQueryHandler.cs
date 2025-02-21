using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Application.Features.AddProduct;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Features.SearchProducts;

public class SearchProductsQueryHandler(ILogger<CreateProductCommandHandler> logger,
    IProductRepository productRepository)
    : IValidatedQueryHandler<SearchProductsQuery, IEnumerable<Product>>
{
    public async Task<Either<IEnumerable<Product>, ValidationResult>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await productRepository.SearchAsync(request.Name, request.Categories,cancellationToken);
            return products;
        }
        catch (Exception exception)
        {
            logger.LogError(exception,"Error occured in {Handler}",nameof(SearchProductsQueryHandler));
            return new ValidationResult("Can't search products");
        }
    }
}