using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Microsoft.Extensions.Logging;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Products;

public record CreateProductCommand(string Name,List<int> Categories,double Price,int Quantity,string ImageBase64) : IValidatedCommand<Product>;

public class CreateProductCommandHandler : IValidatedCommandHandler<CreateProductCommand, Product>
{
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly IProductRepository _productRepository;
    private readonly IImageService _imageService;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger,IProductRepository productRepository,
        IImageService imageService,IProductCategoryRepository productCategoryRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _imageService = imageService;
        _productCategoryRepository = productCategoryRepository;
    }
    
    public async Task<Either<Product, ValidationResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = Guid.NewGuid();
            var imageUrl = await _imageService.UploadAsync(id,request.ImageBase64,cancellationToken);
            var categories = await _productCategoryRepository.GetAsync(request.Categories.ToArray(), cancellationToken);

            var product = new Product(id,request.Name, request.Quantity, request.Price, imageUrl, categories);
            
            var result = await _productRepository.AddAsync(product,cancellationToken);
            await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler} for Request: {@Request}",
                nameof(CreateProductCommandHandler), request);
            return new ValidationResult("Can't create product");
        }
    }
}