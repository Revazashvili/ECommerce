using Orleans.Core;

namespace Basket.API.Grains;

public class BasketGrain(
        [PersistentState(stateName: "basket", storageName: "baskets")]
        IStorage<Models.Basket> state)
    : Grain, IBasketGrain
{
    public Task<Models.Basket> GetAsync() => Task.FromResult(state.State);

    public async Task<Models.Basket?> UpdateAsync(Models.Basket basket)
    {
        state.State = basket;
        await state.WriteStateAsync();
        return basket;
    }

    public Task DeleteAsync() => state.ClearStateAsync();
    
    public Task RemoveItemsByProductId(Guid productId)
    {
        state.State.Items.RemoveAll(item => item.ProductId == productId);
        return state.WriteStateAsync();
    }
}