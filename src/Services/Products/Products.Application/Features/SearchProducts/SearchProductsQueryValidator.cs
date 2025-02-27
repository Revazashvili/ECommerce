using FluentValidation;
using Products.Application.Repositories;

namespace Products.Application.Features.SearchProducts;

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