using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;
using Services.Common.Domain;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _context;

    public OrderRepository(OrderingContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;
    
    public Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken)
    {
        return _context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .FirstOrDefaultAsync(order => order.OrderNumber == orderNumber, cancellationToken);
    }
}