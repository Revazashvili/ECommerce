using Microsoft.AspNetCore.Http;

namespace Ordering.Application.Services;

public interface IIdentityService
{
    Guid GetUserId();
}

public class IdentityService(IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public Guid GetUserId()
    {
        var value = httpContextAccessor.HttpContext!.User.FindFirst("sub")!.Value;
        return Guid.Parse(value);
    }
}