using Ordering.Domain.Entities;

namespace Ordering.Application.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken);
    Task<List<Order>> GetOrdersAsync(DateTime from, DateTime to, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<List<Order>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Guid>> GetNewOrdersOrderNumbersAsync();
}