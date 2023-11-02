using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Services.DependencyInjection;

public static class SwaggerServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services,IConfiguration configuration)
    {
        var swagger = configuration.GetSection("Swagger");
        services.AddSwaggerGen(options =>
        {
            var version = swagger["Version"];
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = swagger["Title"],
                Version = version,
                Description = swagger["Description"]
            });

            var identitySection = configuration.GetSection("Identity");

            var url = identitySection["Url"];
            var scopes = identitySection.GetRequiredSection("Scopes")
                .GetChildren()
                .ToDictionary(p => p.Key, p => p.Value);

            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri($"{url}/connect/authorize"),
                        TokenUrl = new Uri($"{url}/connect/token"),
                        Scopes = scopes
                    }
                }
            });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }
}

internal class AuthorizeCheckOperationFilter : IOperationFilter
{
    private readonly IConfiguration _configuration;

    public AuthorizeCheckOperationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasAuthorize = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(m => m is AuthorizeAttribute);
        if (!hasAuthorize) 
            return;

        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

        var oAuthScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
        };

        var identitySection = _configuration.GetSection("Identity");
        var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().Select(r => r.Key).ToArray();

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                [ oAuthScheme ] = scopes
            }
        };
    }
}