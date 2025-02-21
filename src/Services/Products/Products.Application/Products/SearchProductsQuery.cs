using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Products.Application.Features.AddProduct;
using Products.Domain.Entities;
using Products.Domain.Models;

namespace Products.Application.Products;

public record SearchProductsQuery(string Name,List<int> Categories) : IValidatedQuery<IEnumerable<Product>>;

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

public class SearchProductsQueryValidator : AbstractValidator<SearchProductsQuery>
{
    public SearchProductsQueryValidator(IProductCategoryRepository productCategoryRepository)
    {
        RuleFor(command => command.Name)
            .NotNull()
            .WithMessage("Name must not be null.")
            .NotEmpty()
            .WithMessage("Name must not be empty.");
        
        RuleFor(command => command.Categories)
            .NotNull()
            .WithMessage("Categories must not be null.")
            .NotEmpty()
            .WithMessage("Categories must not be empty.")
            .MustAsync(async (command, context, cancellationToken) =>
            {
                var categories = await productCategoryRepository.GetAsync(command.Categories.ToArray(), cancellationToken);
                return categories.Any();
            })
            .WithMessage("Categories doesn't exists with provided ids");
    }
}