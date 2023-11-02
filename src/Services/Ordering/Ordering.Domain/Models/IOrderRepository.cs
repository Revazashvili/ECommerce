using Ordering.Domain.Entities;
using Services.Common;
using Services.Common.Domain;

namespace Ordering.Domain.Models;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> AddAsync(Order order);
    Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken);
    Task<List<Order>> GetOrdersAsync(DateTime from, DateTime to, Pagination pagination,CancellationToken cancellationToken);
    Task<List<Order>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken);
    Task<List<Guid>> GetNewOrdersOrderNumbersAsync();
}