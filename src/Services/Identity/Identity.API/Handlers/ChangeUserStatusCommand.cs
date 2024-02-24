using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Handlers;

public record ChangeUserStatusCommand(string Id,ApplicationUserStatus Status) : IValidatedCommand<ApplicationUserResponse>;

public class ChangeUserStatusCommandHandler(ILogger<ChangeUserStatusCommandHandler> logger,
        UserManager<ApplicationUser> userManager)
    : IValidatedCommandHandler<ChangeUserStatusCommand, ApplicationUserResponse>
{
    public async Task<Either<ApplicationUserResponse, ValidationResult>> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userManager.FindByIdAsync(request.Id);
            if (user is null)
                return new ValidationResult("can't find user");

            user.Status = request.Status;

            await userManager.UpdateAsync(user);

            return new ApplicationUserResponse(user.Id,
                user.UserName, user.Email,
                user.EmailConfirmed, user.PhoneNumber,
                user.PersonalNumber, user.Status);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Error occured in {Handler}", nameof(GetAllUsersQueryHandler));
            return new ValidationResult("Can't change user status");
        }
    }
}