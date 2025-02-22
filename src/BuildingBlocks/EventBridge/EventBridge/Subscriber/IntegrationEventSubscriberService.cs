using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBridge.Subscriber;

public abstract class IntegrationEventSubscriberService : IHostedService
{
    private readonly SubscriberEventProcessFunctionStore _subscriberEventProcessFunctionStore;
    private readonly IServiceProvider _serviceProvider;

    protected IntegrationEventSubscriberService(IServiceProvider serviceProvider)
    {
        _subscriberEventProcessFunctionStore = _serviceProvider.GetRequiredService<SubscriberEventProcessFunctionStore>();
        _serviceProvider = serviceProvider;
    }

    protected abstract Task Subscribe(string topic, ProcessEvent processEventFunction, CancellationToken cancellationToken);
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var processEvents = _subscriberEventProcessFunctionStore.GetProcessEventFunctions();

        var tasks = processEvents.Select(pair => Subscribe(pair.Key, pair.Value, cancellationToken)).ToList();
        
        await Task.WhenAll(tasks);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}