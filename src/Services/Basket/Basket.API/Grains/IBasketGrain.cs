namespace Basket.API.Grains;

public interface IBasketGrain : IGrainWithStringKey
{
    Task<Models.Basket> GetAsync();
    Task<Models.Basket?> UpdateAsync(Models.Basket basket);
    Task DeleteAsync();
    Task RemoveItemsByProductId(Guid productId);
}