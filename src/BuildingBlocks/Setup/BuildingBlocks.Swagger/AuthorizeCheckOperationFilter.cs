using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildingBlocks.Swagger;

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