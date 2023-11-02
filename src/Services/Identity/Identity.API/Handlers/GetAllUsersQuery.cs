using Contracts;
using Contracts.Mediatr.Validation;
using Contracts.Mediatr.Wrappers;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Handlers;

public record GetAllUsersQuery : IValidatedQuery<IEnumerable<ApplicationUserResponse>>;

public class GetAllUsersQueryHandler : IValidatedQueryHandler<GetAllUsersQuery, IEnumerable<ApplicationUserResponse>>
{
    private readonly ILogger<GetAllUsersQueryHandler> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetAllUsersQueryHandler(ILogger<GetAllUsersQueryHandler> logger,UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }
    
    public async Task<Either<IEnumerable<ApplicationUserResponse>, ValidationResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userManager.Users
                .Select(user => new ApplicationUserResponse(user.Id,
                    user.UserName, user.Email,
                    user.EmailConfirmed, user.PhoneNumber,
                    user.PersonalNumber, user.Status))
                .ToListAsync(cancellationToken);

            return users;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error occured in {Handler}", nameof(GetAllUsersQueryHandler));
            return new ValidationResult("Can't retrieve users");
        }
    }
}