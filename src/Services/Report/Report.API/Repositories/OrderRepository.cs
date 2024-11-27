using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Core.Search;
using Report.API.Models;

namespace Report.API.Repositories;

public class OrderRepository(ElasticsearchClient elasticClient) : IOrderRepository
{
    public async Task<IEnumerable<SalesReport>> GetSalesReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        var response = await elasticClient.SearchAsync<Order>(descriptor =>
            descriptor
                .Source(new SourceConfig(new SourceFilter
                {
                    Includes = Infer.Field<Order>(order => order.OrderItems),
                    Excludes = Infer.Fields<Order>(order => order.OrderingDate,
                        order => order.OrderNumber,
                        order => order.OrderStatus,
                        order => order.Address,
                        order => order.UserId)
                }))
                .Query(queryDescriptor => queryDescriptor
                    .Range(rangeQueryDescriptor => rangeQueryDescriptor.DateRange(dateRangeQueryDescriptor =>
                    {
                        dateRangeQueryDescriptor.Gte(from);
                        dateRangeQueryDescriptor.Lte(to);
                    }))),
            cancellationToken
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

    public async Task<Order?> AddAsync(Order order, CancellationToken cancellationToken)
    {
        var response = await elasticClient.IndexAsync(order, cancellationToken);

        return response.IsValidResponse ? order : null;
    }
}