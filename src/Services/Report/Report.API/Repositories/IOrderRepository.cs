using Report.API.Models;

namespace Report.API.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<SalesReport>> GetSalesReportAsync(DateTime from, DateTime to);
    Task<Order?> AddAsync(Order order);
}