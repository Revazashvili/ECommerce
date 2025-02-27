using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BuildingBlocks.Swagger;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services,
        IConfiguration configuration,
        string swaggerSectionName,
        string identitySectionName)
    {
        var swagger = configuration.GetSection(swaggerSectionName);
        services.AddSwaggerGen(options =>
        {
            var version = swagger["Version"];
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = swagger["Title"],
                Version = version,
                Description = swagger["Description"]
            });

            var identitySection = configuration.GetSection(identitySectionName);

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