namespace Basket.API.Interfaces;

public interface IBasketRepository
{
    Task<IEnumerable<string>> GetAllKeysAsync();
}