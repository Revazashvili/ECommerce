using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Products;

public record GetProductsQuery : IValidatedQuery<IEnumerable<Product>>;

public class GetProductsQueryHandler : IValidatedQueryHandler<GetProductsQuery,IEnumerable<Product>>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(ILogger<CreateProductCommandHandler> logger,IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }
    
    public async Task<Either<IEnumerable<Product>, ValidationResult>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.GetAsync(cancellationToken);
            return products;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}", nameof(GetProductsQueryHandler));
            return new ValidationResult("Can't return product list");
        }
    }
}