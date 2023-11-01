using Report.API.Models;

namespace Report.API.Repositories;

public class OrderRepository : IOrderRepository
{
    public Task<Order> AddAsync(Order order)
    {
        throw new NotImplementedException();
    }
}