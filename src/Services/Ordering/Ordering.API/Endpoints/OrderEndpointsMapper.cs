using Contracts;
using Contracts.Mediatr.Validation;
using MediatR;
using Ordering.Application.Orders;
using Services.Common;

namespace Ordering.API.Endpoints;

internal static class OrderEndpointsMapper
{
    internal static void MapOrder(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("order");

        // basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
        //         ISender sender) =>
        //     {
        //         var result = await sender.Send(new GetProductsQuery(), cancellationToken);
        //         return result.ToResult();
        //     }).Produces<IEnumerable<Product>>()
        //     .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        //
        // basketRouteGroupBuilder.MapPost("/search", async (SearchProductsQuery query ,CancellationToken cancellationToken,
        //         ISender sender) =>
        //     {
        //         var result = await sender.Send(new SearchProductsQuery(query.Name,query.Categories), cancellationToken);
        //         return result.ToResult();
        //     }).Produces<IEnumerable<Product>>()
        //     .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        //
        // basketRouteGroupBuilder.MapPost("/", async (CreateProductCommand command, CancellationToken cancellationToken,
        //         ISender sender) =>
        //     {
        //         var result = await sender.Send(command, cancellationToken);
        //         return result.ToResult();
        //     })
        //     .Produces<Product>()
        //     .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        //
        
        basketRouteGroupBuilder.MapPut("/cancel", async (CancelOrderCommand command, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.ToResult();
            })
            .Produces<None>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}