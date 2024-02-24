using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Products;

public record CreateProductCommand(string Name,List<int> Categories,double Price,int Quantity,string ImageBase64) : IValidatedCommand<Product>;

public class CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger,
        IProductRepository productRepository,
        IImageService imageService, IProductCategoryRepository productCategoryRepository)
    : IValidatedCommandHandler<CreateProductCommand, Product>
{
    public async Task<Either<Product, ValidationResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var id = Guid.NewGuid();
            var imageUrl = await imageService.UploadAsync(id,request.ImageBase64,cancellationToken);
            var categories = await productCategoryRepository.GetAsync(request.Categories.ToArray(), cancellationToken);

            var product = new Product(id,request.Name, request.Quantity, request.Price, imageUrl, categories);
            
            var result = await productRepository.AddAsync(product,cancellationToken);
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

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductCategoryRepository productCategoryRepository)
    {
        RuleFor(command => command.Name)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
        
        RuleFor(command => command.ImageBase64)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
        
        RuleFor(command => command.Quantity)
            .NotNull()
            .WithMessage("Quantity must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Quantity must be greater or equal to 1.");
        
        RuleFor(command => command.Price)
            .NotNull()
            .WithMessage("Price must not be null.")
            .GreaterThanOrEqualTo(1)
            .WithMessage("Price must be greater or equal to 1.");
        
        RuleFor(command => command.Categories)
            .NotNull()
            .WithMessage("Categories must not be null.")
            .NotEmpty()
            .WithMessage("Categories must not be empty.")
            .MustAsync(async (command, context, cancellationToken) =>
            {
                var categories = await productCategoryRepository.GetAsync(command.Categories.ToArray(), cancellationToken);
                return categories.Any();
            });
    }
}