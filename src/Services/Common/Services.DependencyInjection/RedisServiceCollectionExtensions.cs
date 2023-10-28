using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Services.DependencyInjection;

public static class RedisServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisSection = configuration.GetSection("Redis");
        
        services.AddStackExchangeRedisCache(options =>
        {
            // options.Configuration = builder.Configuration.GetConnectionString("MyRedisConStr");
            options.Configuration = redisSection["Url"];
            options.InstanceName = redisSection["InstanceName"];
        });

        return services;
    }
}