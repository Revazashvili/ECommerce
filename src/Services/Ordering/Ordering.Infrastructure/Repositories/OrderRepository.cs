using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;
using Ordering.Domain.Models;
using Services.Common;
using Services.Common.Domain;
using Services.Common.Extensions;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository(OrderingContext context) : IOrderRepository
{
    public IUnitOfWork UnitOfWork => context;

    public async Task<Order> AddAsync(Order order)
    {
        var entityEntry = await context.Orders.AddAsync(order);
        return entityEntry.Entity;
    }

    public Task<Order?> GetByOrderNumberAsync(Guid orderNumber, CancellationToken cancellationToken)
    {
        return context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .FirstOrDefaultAsync(order => order.OrderNumber == orderNumber, cancellationToken);
    }

    public Task<List<Order>> GetOrdersAsync(DateTime from, DateTime to, Pagination pagination, CancellationToken cancellationToken)
    {
        return context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .Where(order => order.OrderingDate >= from && order.OrderingDate <= to)
            .Paged(pagination)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public Task<List<Order>> GetUserOrdersAsync(Guid userId, CancellationToken cancellationToken)
    {
        return context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .Where(order => order.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public Task<List<Guid>> GetNewOrdersOrderNumbersAsync()
    {
        return context.Orders
            .Include(order => order.OrderItems)
            .Include(order => order.Address)
            .Where(order => order.OrderStatus == OrderStatus.Created)
            .Select(order => order.OrderNumber)
            .ToListAsync();
    }
}