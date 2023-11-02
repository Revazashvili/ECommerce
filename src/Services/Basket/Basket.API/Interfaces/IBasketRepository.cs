namespace Basket.API.Interfaces;

public interface IBasketRepository
{
    Task<IEnumerable<string>> GetAllKeysAsync();
    Task<Models.Basket?> GetBasketAsync(string key,CancellationToken cancellationToken);
    Task CreateOrUpdateBasketAsync(string key,Models.Basket basket);
    Task DeleteBasketAsync(string key,CancellationToken cancellationToken);
}