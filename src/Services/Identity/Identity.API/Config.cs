using Duende.IdentityServer.Models;

namespace Identity.API;

public static class Config
{
    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>
        {
            new("basket", "Basket Service"),
            new("products", "Products Service"),
            new("ordering", "Ordering Service"),
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
            new("basket"),
            new("products"),
            new("ordering")
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
            new Client
            {
                ClientId = "products",
                ClientName = "Products API",
                ClientSecrets = { new Secret("75DBB9C4-4E42-46ED-A012-298A88B8290A".Sha256()) },

                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{apisSection["Products"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{apisSection["Products"]}/swagger" },

                AllowedScopes = {  "openid", "profile", "products" }
            },
            new Client
            {
                ClientId = "ordering",
                ClientName = "Ordering API",
                ClientSecrets = { new Secret("734A6DA8-F5DA-4B65-842E-FF13F82A550C".Sha256()) },

                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,

                RedirectUris = { $"{apisSection["Ordering"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{apisSection["Ordering"]}/swagger" },

                AllowedScopes = {  "openid", "profile", "ordering" }
            },
        };
    }
}