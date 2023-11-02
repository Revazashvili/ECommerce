using Basket.API.Handlers;
using Basket.API.Services;
using Contracts;
using Contracts.Mediatr.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Common;

namespace Basket.API.Endpoints;

internal static class BasketEndpointsMapper
{
    internal static void MapBasket(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("basket");

        basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
                [FromServices]ISender sender,[FromServices]IIdentityService identityService) =>
            {
                var result = await sender.Send(new GetBasketQuery(), cancellationToken);
                return result.ToResult();
            }).Produces<Models.Basket>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        basketRouteGroupBuilder.MapPost("/", async (Models.Basket basket, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new UpdateBasketCommand(basket), cancellationToken);
                return result.ToResult();
            })
            .Produces<Models.Basket>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        
        basketRouteGroupBuilder.MapDelete("/", async (CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(), cancellationToken);
                return result.ToResult();
            })
            .Produces<None>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}