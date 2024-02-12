namespace Basket.API.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor)
    : IIdentityService
{
    public string GetUserId() => 
        httpContextAccessor.HttpContext!.User.FindFirst("sub")!.Value;
}