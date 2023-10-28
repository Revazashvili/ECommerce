namespace Basket.API.Interfaces;

public interface IBasketRepository
{
    Task<Models.Basket?> GetBasketAsync(string key,CancellationToken cancellationToken);
    Task CreateOrUpdateBasketAsync(Models.Basket basket);
    Task DeleteBasketAsync(string key,CancellationToken cancellationToken);
}