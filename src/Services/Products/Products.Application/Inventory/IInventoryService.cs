using Refit;

namespace Products.Application.Inventory;

public interface IInventoryService
{
    [Post("/api/stock/reserve")]
    Task ReserveAsync(ReserveRequest request, CancellationToken cancellationToken);
}