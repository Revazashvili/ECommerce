using Microsoft.AspNetCore.Http;

namespace Ordering.Application.Services;

public interface IIdentityService
{
    Guid GetUserId();
}

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var value = _httpContextAccessor.HttpContext!.User.FindFirst("sub")!.Value;
        return Guid.Parse(value);
    }
}