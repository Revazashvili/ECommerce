using System.Net;
using System.Text.Json;
using Basket.API.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Basket.API.Repositories;

public class RedisBasketRepository : IBasketRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly IConfiguration _configuration;

    public RedisBasketRepository(IDistributedCache distributedCache,IConfiguration configuration)
    {
        _distributedCache = distributedCache;
        _configuration = configuration;
    }

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
        var redisSection = _configuration.GetSection("Redis");
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

    public async Task<Models.Basket?> GetBasketAsync(string key,CancellationToken cancellationToken)
    {
        var value = await _distributedCache.GetAsync(key, cancellationToken);
        return value is null
            ? null
            : await JsonSerializer.DeserializeAsync<Models.Basket>(new MemoryStream(value), cancellationToken: cancellationToken);
    }

    public async Task CreateOrUpdateBasketAsync(Models.Basket basket)
    {
        var json = JsonSerializer.Serialize(basket);
        await _distributedCache.SetStringAsync(basket.UserId.ToString(), json);
    }

    public async Task DeleteBasketAsync(string key, CancellationToken cancellationToken) =>
        await _distributedCache.RemoveAsync(key, cancellationToken);
}