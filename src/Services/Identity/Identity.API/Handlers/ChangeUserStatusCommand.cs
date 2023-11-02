using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Handlers;

public record ChangeUserStatusCommand(string Id,ApplicationUserStatus Status) : IValidatedCommand<ApplicationUserResponse>;

public class ChangeUserStatusCommandHandler : IValidatedCommandHandler<ChangeUserStatusCommand, ApplicationUserResponse>
{
    private readonly ILogger<ChangeUserStatusCommandHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public ChangeUserStatusCommandHandler(ILogger<ChangeUserStatusCommandHandler> logger,UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Either<ApplicationUserResponse, ValidationResult>> Handle(ChangeUserStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null)
                return new ValidationResult("can't find user");

            user.Status = request.Status;

            await _userManager.UpdateAsync(user);

            return new ApplicationUserResponse(user.Id,
                user.UserName, user.Email,
                user.EmailConfirmed, user.PhoneNumber,
                user.PersonalNumber, user.Status);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}", nameof(GetAllUsersQueryHandler));
            return new ValidationResult("Can't change user status");
        }
    }
}