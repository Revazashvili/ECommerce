using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using EventBridge.Dispatcher;
using Microsoft.Extensions.Logging;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Application.Repositories;

namespace Products.Application.Features.AddProduct;

public class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger,
    IProductRepository productRepository,
    IImageService imageService, 
    IProductCategoryRepository productCategoryRepository,
    IIntegrationEventDispatcher dispatcher)
    : IValidatedCommandHandler<CreateProductCommand, Product>
{
    public async Task<Either<Product, ValidationResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = Guid.NewGuid();
            var imageUrl = await imageService.UploadAsync(id,request.ImageBase64,cancellationToken);
            var categories = await productCategoryRepository.GetAsync(request.Categories.ToArray(), cancellationToken);

            var product = Product.Create(id, request.Name, request.Price, imageUrl, categories);
            
            var result = await productRepository.AddAsync(product,cancellationToken);

            await dispatcher.DispatchAsync(
                "ProductAdded",
                new ProductAddedIntegrationEvent(product.Id, product.Name),
                cancellationToken
            );
            
            await productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler} for Request: {@Request}",
                nameof(CreateProductCommandHandler), request);
            return new ValidationResult("Can't create product");
        }
    }
}