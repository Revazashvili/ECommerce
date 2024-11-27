using Microsoft.AspNetCore.Mvc;
using Report.API.Models;
using Report.API.Repositories;

namespace Report.API.Endpoints;

internal static class ReportEndpointsMapper
{
    internal static void MapReport(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        var basketRouteGroupBuilder = endpointRouteBuilder.MapGroup("report");
        
        basketRouteGroupBuilder.MapGet("/{from:datetime}", async (DateTime from, IOrderRepository orderRepository, CancellationToken cancellationToken) => 
                await orderRepository.GetSalesReportAsync(from,DateTime.Now, cancellationToken))
            .Produces<IEnumerable<Order>>();
        
        basketRouteGroupBuilder.MapGet("/{period}", async ([FromQuery]SalesReportPeriod period, IOrderRepository orderRepository, CancellationToken cancellationToken) => 
                await orderRepository.GetSalesReportAsync(period.CalculateFromDate(),DateTime.Now, cancellationToken))
            .Produces<IEnumerable<Order>>();
        
        basketRouteGroupBuilder.MapPost("/", async (Order order,IOrderRepository orderRepository, CancellationToken cancellationToken) => 
            await orderRepository.AddAsync(order, cancellationToken))
            .Produces<IEnumerable<Order>>();
    }
}