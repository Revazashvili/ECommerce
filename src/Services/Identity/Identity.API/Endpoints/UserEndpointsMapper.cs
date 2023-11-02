using Duende.IdentityServer.Validation;
using Identity.API.Handlers;
using Identity.API.Models;
using MediatR;
using Services.Common;

namespace Identity.API.Endpoints;

internal static class UserEndpointsMapper
{
    internal static void MapUser(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("user");

        basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
            ISender sender) =>
            {
                var result = await sender.Send(new GetAllUsersQuery(), cancellationToken);
                return result.ToResult();
            }).Produces<IEnumerator<ApplicationUserResponse>>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        basketRouteGroupBuilder.MapPut("/activate/{id}", async (string id, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(
                    new ChangeUserStatusCommand(id,ApplicationUserStatus.Active), cancellationToken);
                return result.ToResult();
            })
            .Produces<ApplicationUserResponse>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        basketRouteGroupBuilder.MapPut("/deactivate/{id}", async (string id, CancellationToken cancellationToken,
                ISender sender) =>
            {
                var result = await sender.Send(
                    new ChangeUserStatusCommand(id,ApplicationUserStatus.Passive), cancellationToken);
                return result.ToResult();
            })
            .Produces<ApplicationUserResponse>()
            .Produces<ValidationResult>(StatusCodes.Status400BadRequest);

        
        // basketRouteGroupBuilder.MapGet("/", async (CancellationToken cancellationToken,
        //         [FromServices]ISender sender,[FromServices]IIdentityService identityService) =>
        //     {
        //         var result = await sender.Send(new GetBasketQuery(), cancellationToken);
        //         return result.ToResult();
        //     }).Produces<Models.Basket>()
        //     .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        //
        // basketRouteGroupBuilder.MapPost("/", async (Models.Basket basket, CancellationToken cancellationToken,
        //         ISender sender) =>
        //     {
        //         var result = await sender.Send(new UpdateBasketCommand(basket), cancellationToken);
        //         return result.ToResult();
        //     })
        //     .Produces<Models.Basket>()
        //     .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
        //
        // basketRouteGroupBuilder.MapDelete("/", async (CancellationToken cancellationToken,
        //         ISender sender) =>
        //     {
        //         var result = await sender.Send(new DeleteBasketCommand(), cancellationToken);
        //         return result.ToResult();
        //     })
        //     .Produces<None>()
        //     .Produces<ValidationResult>(StatusCodes.Status400BadRequest);
    }
}