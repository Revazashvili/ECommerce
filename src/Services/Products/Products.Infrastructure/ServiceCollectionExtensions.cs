using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Repositories;
using Products.Infrastructure.Repositories;

namespace Products.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(ProductsContext));

        services.AddDbContext<ProductsContext>(builder =>
        {
            builder.UseNpgsql(connectionString, optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(ProductsContext).Assembly.FullName);
                });
            builder.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, ProductsContext>();
        services.AddOutboxDispatcher(provider =>
        {
            var orderingContext = provider.GetRequiredService<ProductsContext>();

            return new OutboxMessageRepository(orderingContext);
        });
        
        return services;
    }
}