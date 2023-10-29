using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;
using Services.Common;
using Services.Common.Domain;
using Services.Common.Extensions;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _context;

    public OrderRepository(OrderingContext context)
    {
        _context = context;
    }

    public IUnitOfWork UnitOfWork => _context;

    public async Task<Order> AddAsync(Order order)
    {
        var entityEntry = await _context.Orders.AddAsync(order);
        return entityEntry.Entity;
    }

    public Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken)
    {
        return _context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .FirstOrDefaultAsync(order => order.OrderNumber == orderNumber, cancellationToken);
    }

    public Task<List<Order>> GetOrdersAsync(DateTime from, DateTime to, Pagination pagination, CancellationToken cancellationToken)
    {
        return _context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .Where(order => order.OrderingDate >= from && order.OrderingDate <= to)
            .Paged(pagination)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}