using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Domain.Models;
using Ordering.Infrastructure.Repositories;
using Services.Common.Domain;

namespace Ordering.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(OrderingContext));

        services.AddDbContext<OrderingContext>(builder => builder.UseSqlServer(connectionString,
            optionsBuilder =>
            {
                optionsBuilder.MigrationsAssembly(typeof(OrderingContext).Assembly.FullName);
            }));

        services.AddScoped<IUnitOfWork, OrderingContext>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        
        return services;
    }
}