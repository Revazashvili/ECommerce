using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Services.DependencyInjection;

public static class DbContextMigrationApplicationBuilderExtensions
{
    public static IApplicationBuilder MigrateIfDevelopmentAsync<T>(this WebApplication app)
        where T : DbContext
    {
        if (!app.Environment.IsDevelopment())
            return app;
        
        using var scope = app.Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<T>();
        dbContext.Database.MigrateAsync();
        return app;
    }
}