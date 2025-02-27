using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Setup;

public static class SwaggerApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwagger(this WebApplication app, IConfiguration configuration)
    {
        var swaggerSection = configuration.GetSection("Swagger");
        
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