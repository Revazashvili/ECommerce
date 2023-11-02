using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services.DependencyInjection;

public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddHttpContextAccessor();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        var identitySection = configuration.GetSection("Identity");
        services.AddAuthentication().AddJwtBearer(options =>
        {

            options.Authority = identitySection["Url"];
            options.RequireHttpsMetadata = false;
            options.Audience = identitySection["Audience"];
            options.TokenValidationParameters.ValidateAudience = false;
        });

        return services;
    }
}