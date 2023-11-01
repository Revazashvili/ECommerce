using Report.API.Models;

namespace Report.API.Repositories;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order);
}