using System.Text.Json;
using Basket.API.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Repositories;

public class RedisBasketRepository : IBasketRepository
{
    private readonly IDistributedCache _distributedCache;

    public RedisBasketRepository(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
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