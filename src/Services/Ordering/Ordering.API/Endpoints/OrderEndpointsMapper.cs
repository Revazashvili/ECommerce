using Contracts;
using Contracts.Mediatr.Validation;
using MediatR;
using Ordering.Application.Orders;
using Ordering.Domain.Entities;
using Services.Common;

namespace Ordering.API.Endpoints;

internal static class OrderEndpointsMapper
{
    internal static void MapOrder(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var orderRouteGroupBuilder = endpointRouteBuilder.MapGroup("order");

        orderRouteGroupBuilder.MapGet("/{from:datetime}/{to:datetime}/{pageNumber:int}/{pageSize:int}",
                async (DateTime from, DateTime to, int pageNumber, int pageSize,
                    CancellationToken cancellationToken, ISender sender) =>
                {
                    var pagination = new Pagination(pageNumber, pageSize);
                    var query = new GetOrdersQuery(from, to, pagination);
                    var result = await sender.Send(query, cancellationToken);
                    return result.ToResult();
                }).Produces<IEnumerable<Order>>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        
        orderRouteGroupBuilder.MapGet("/{orderNumber:guid}", async (Guid orderNumber,
                CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByOrderNumberQuery(orderNumber), cancellationToken);
                return result.ToResult();
            }).Produces<Order>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
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
        
        orderRouteGroupBuilder.MapPut("/cancel", async (CancelOrderCommand command, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(command, cancellationToken);
                return result.ToResult();
            })
            .Produces<None>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}