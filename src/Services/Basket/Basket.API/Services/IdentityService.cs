namespace Basket.API.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId() => 
        _httpContextAccessor.HttpContext!.User.FindFirst("sub")!.Value;
}