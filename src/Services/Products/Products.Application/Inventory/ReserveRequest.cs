namespace Products.Application.Inventory;

public record ReserveRequest(Guid OrderNumber, List<ProductToReserve> Products);

public record ProductToReserve(Guid ProductId, int Quantity);