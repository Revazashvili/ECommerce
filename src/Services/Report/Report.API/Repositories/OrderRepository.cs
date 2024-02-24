using Nest;
using Report.API.Models;

namespace Report.API.Repositories;

public class OrderRepository(IElasticClient elasticClient) : IOrderRepository
{
    public async Task<IEnumerable<SalesReport>> GetSalesReportAsync(DateTime from, DateTime to)
    {
        var response = await elasticClient
            .SearchAsync<Order>(descriptor =>
                descriptor
                    .Size(10000)
                    .Source(sf => sf
                        .Includes(i => i
                            .Field(order => order.OrderItems)
                        )
                        .Excludes(e => e
                            .Fields(
                                order => order.OrderingDate,
                                order => order.OrderNumber,
                                order => order.OrderStatus,
                                order => order.Address,
                                order => order.UserId)
                        )
                    ).Query(containerDescriptor =>
                        containerDescriptor.DateRange(queryDescriptor =>
                            queryDescriptor.GreaterThanOrEquals(from).LessThanOrEquals(to)
                        )
                    )
            );

        var salesReports = response.Documents
            .SelectMany(x => x.OrderItems)
            .GroupBy(item => item.ProductName)
            .Select(items =>
                new SalesReport(items.Key,
                    items.Sum(item => item.Quantity),
                    items.Sum(item => item.Price)))
            .ToList();

        return salesReports;
    }

    public async Task<Order?> AddAsync(Order order)
    {
        var response = await elasticClient.IndexDocumentAsync(order);
        return response.Result == Result.Error ? null : order;
    }
}