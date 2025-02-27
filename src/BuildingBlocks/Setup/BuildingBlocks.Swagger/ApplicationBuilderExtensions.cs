using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Swagger;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwagger(this WebApplication app, 
        IConfiguration configuration,
        string swaggerSectionName)
    {
        var swaggerSection = configuration.GetSection(swaggerSectionName);
        
        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            setup.OAuthClientId(swaggerSection["ClientId"]);
            setup.OAuthAppName(swaggerSection["AppName"]);
        });

        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

        return app;
    }
}