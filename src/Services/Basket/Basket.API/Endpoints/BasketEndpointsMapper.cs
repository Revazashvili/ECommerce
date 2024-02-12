using Basket.API.Grains;
using Basket.API.Services;
using Contracts;
using Contracts.Mediatr.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Endpoints;

internal static class BasketEndpointsMapper
{
    internal static void MapBasket(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("basket");

        basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
                [FromServices]IGrainFactory grainFactory,
                [FromServices]IIdentityService identityService) =>
            {
                var userId = identityService.GetUserId();
                var basketGrain = grainFactory.GetGrain<IBasketGrain>(userId);
                var result = await basketGrain.GetAsync();
                return Results.Ok(result);
            }).Produces<Models.Basket>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        basketRouteGroupBuilder.MapPost("/", async ([FromBody]Models.Basket basket, 
                [FromServices]IGrainFactory grainFactory,
                [FromServices]IIdentityService identityService) =>
            {
                var userId = identityService.GetUserId();
                var basketGrain = grainFactory.GetGrain<IBasketGrain>(userId);
                var result = await basketGrain.UpdateAsync(basket);
                return Results.Ok(result);
            })
            .Produces<Models.Basket>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        
        basketRouteGroupBuilder.MapDelete("/", async (
                [FromServices]IGrainFactory grainFactory,
                [FromServices]IIdentityService identityService) =>
            {
                var userId = identityService.GetUserId();
                var basketGrain = grainFactory.GetGrain<IBasketGrain>(userId);
                await basketGrain.DeleteAsync();
                return Results.Ok();
            })
            .Produces<None>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}