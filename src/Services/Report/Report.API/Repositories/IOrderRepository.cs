using Report.API.Models;

namespace Report.API.Repositories;

public interface IOrderRepository
{
    Task<IEnumerable<SalesReport>> GetSalesReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
    Task<Order?> AddAsync(Order order, CancellationToken cancellationToken);
}