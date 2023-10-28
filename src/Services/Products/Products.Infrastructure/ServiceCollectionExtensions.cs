using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Domain.Models;
using Products.Infrastructure.Repositories;

namespace Products.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(ProductsContext));

        services.AddDbContext<ProductsContext>(builder => builder.UseNpgsql(connectionString,
            optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(ProductsContext).Assembly.FullName);
            }));

        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUnitOfWork, ProductsContext>();
        
        return services;
    }
}