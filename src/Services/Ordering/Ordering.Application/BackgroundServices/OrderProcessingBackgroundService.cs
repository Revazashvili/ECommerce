using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Application.Orders;
using Ordering.Domain.Models;

namespace Ordering.Application.BackgroundServices;

public class OrderProcessingBackgroundService : BackgroundService
{
    private readonly ILogger<OrderProcessingBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public OrderProcessingBackgroundService(ILogger<OrderProcessingBackgroundService> logger,IServiceProvider serviceProvider,
        ISender sender)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Order processing service running.");
        
        try
        {
            // process orders one time not to wait for timer to tick
            await ProcessNewOrdersAsync();
        
            using PeriodicTimer timer = new(TimeSpan.FromSeconds(3));
            
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ProcessNewOrdersAsync();
            }
        }
        catch (OperationCanceledException operationCanceledException)
        {
            _logger.LogError(operationCanceledException, "Timed Hosted Service is stopping.");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception,"Error occured in {BackgroundService}",nameof(OrderProcessingBackgroundService));
        }
    }

    private async Task ProcessNewOrdersAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
        var sender = scope.ServiceProvider.GetRequiredService<ISender>();
        
        var orderNumbers = await orderRepository.GetNewOrdersOrderNumbersAsync();
        foreach (var orderNumber in orderNumbers)
        {
            await sender.Send(new SetPendingStatusCommand(orderNumber));
        }
    }
}