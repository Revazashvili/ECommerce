using Duende.IdentityServer.Models;

namespace Identity.API;

public static class Config
{
    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>
        {
            new ApiResource("basket", "Basket Service"),
        };
    }

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("basket")
        };

    public static IEnumerable<Client> Clients(IConfiguration configuration)
    {
        var apisSection = configuration.GetRequiredSection("APIs");
        return new[]
        {
            new Client
            {
                ClientId = "basket",
                ClientName = "Basket API",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{apisSection["Basket"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{apisSection["Basket"]}/swagger" },

                AllowedScopes = {  "openid", "profile", "basket" }
            },
        };
    }
}