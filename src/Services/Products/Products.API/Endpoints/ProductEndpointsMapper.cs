using Contracts.Mediatr.Validation;
using MediatR;
using Products.API.Extensions;
using Products.Application.Features.AddProduct;
using Products.Application.Features.GetProducts;
using Products.Application.Features.SearchProducts;
using Products.Domain.Entities;

namespace Products.API.Endpoints;

internal static class ProductEndpointsMapper
{
    internal static void MapProduct(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("product");

        basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new GetProductsQuery(), cancellationToken);
                return result.ToResult();
            }).Produces<IEnumerable<Product>>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        
        basketRouteGroupBuilder.MapPost("/search", async (SearchProductsQuery query ,CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new SearchProductsQuery(query.Name,query.Categories), cancellationToken);
                return result.ToResult();
            }).Produces<IEnumerable<Product>>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        basketRouteGroupBuilder.MapPost("/", async (CreateProductCommand command, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.ToResult();
            })
            .Produces<Product>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}