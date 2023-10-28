namespace Basket.API.Interfaces;

public interface IBasketRepository
{
    Task<Models.Basket> GetBasketAsync(string customerId);
    Task<Models.Basket> UpdateBasketAsync(Models.Basket basket);
    Task<bool> DeleteBasketAsync(string id);
}