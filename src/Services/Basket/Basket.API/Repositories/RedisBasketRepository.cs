using Basket.API.Interfaces;
using StackExchange.Redis;

namespace Basket.API.Repositories;

public class RedisBasketRepository(IConfiguration configuration)
    : IBasketRepository
{
    public async Task<IEnumerable<string>> GetAllKeysAsync()
    {
        var server = await GetServer();
        var instanceName = GetRedisConfigurationValue("InstanceName");
        return server.Keys()
            .Select(key => key.ToString()!.Replace(instanceName,""))
            .ToList();
    }

    private string GetRedisConfigurationValue(string key)
    {
        var redisSection = configuration.GetSection("Redis");
        return redisSection[key]!;
    }
    
    private async Task<IServer> GetServer()
    {
        var options = ConfigurationOptions.Parse(GetRedisConfigurationValue("Url"));
        var connection = await ConnectionMultiplexer.ConnectAsync(options);
        var endPoint = connection.GetEndPoints().First();
        var server = connection.GetServer(endPoint);
        return server;
    }
}