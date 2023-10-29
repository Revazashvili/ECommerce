using Ordering.Domain.Entities;
using Services.Common.Domain;

namespace Ordering.Domain.Models;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken);
}