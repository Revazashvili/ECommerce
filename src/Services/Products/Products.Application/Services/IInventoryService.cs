using Refit;

namespace Products.Application.Services;

public record ReserveRequest(Guid OrderNumber, List<ProductToReserve> Products);

public record ProductToReserve(Guid ProductId, int Quantity);

public interface IInventoryService
{
    [Post("/api/stock/reserve")]
    Task ReserveAsync(ReserveRequest request, CancellationToken cancellationToken);
}