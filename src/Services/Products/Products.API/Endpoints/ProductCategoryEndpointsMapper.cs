using Contracts;
using Contracts.Mediatr.Validation;
using MediatR;
using Products.Application.Features.AddProductCategory;
using Products.Application.Features.DeleteProductCategory;
using Products.Application.Features.GetProductCategories;
using Products.Application.ProductCategories;
using Products.Domain.Entities;
using Services.Common;

namespace Products.API.Endpoints;

internal static class ProductCategoryEndpointsMapper
{
    internal static void MapProductCategory(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("product/category");

        basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new GetProductCategoriesQuery(), cancellationToken);
                return result.ToResult();
            }).Produces<IEnumerable<ProductCategory>>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        basketRouteGroupBuilder.MapPost("/", async (CreateProductCategoryCommand command, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.ToResult();
            })
            .Produces<ProductCategory>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        
        basketRouteGroupBuilder.MapPut("/", async (UpdateProductCategoryCommand command, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.ToResult();
            })
            .Produces<ProductCategory>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        
        basketRouteGroupBuilder.MapDelete("/{id:int}", async (int id, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCategoryCommand(id), cancellationToken);
                return result.ToResult();
            })
            .Produces<None>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}