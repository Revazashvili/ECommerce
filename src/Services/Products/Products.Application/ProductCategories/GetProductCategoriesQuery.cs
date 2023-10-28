using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.ProductCategories;

public record GetProductCategoriesQuery : IValidatedQuery<IEnumerable<ProductCategory>>;

public class GetProductCategoriesQueryHandler : IValidatedQueryHandler<GetProductCategoriesQuery,IEnumerable<ProductCategory>>
{
    private readonly ILogger<UpdateProductCategoryCommandHandler> _logger;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public GetProductCategoriesQueryHandler(ILogger<UpdateProductCategoryCommandHandler> logger, IProductCategoryRepository productCategoryRepository)
    {
        _logger = logger;
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task<Either<IEnumerable<ProductCategory>, ValidationResult>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        var productCategories = await _productCategoryRepository.GetAsync(cancellationToken);
        return productCategories;
    }
}