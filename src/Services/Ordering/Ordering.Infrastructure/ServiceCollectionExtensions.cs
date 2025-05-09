using EventBridge.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Repositories;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(nameof(OrderingContext));

        services.AddDbContext<OrderingContext>(builder =>
        {
            builder.UseNpgsql(connectionString, optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(OrderingContext).Assembly.FullName);
                });
            builder.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork, OrderingContext>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddOutboxDispatcher(provider =>
        {
            var orderingContext = provider.GetRequiredService<OrderingContext>();

            return new OutboxMessageRepository(orderingContext);
        });
        
        return services;
    }
}