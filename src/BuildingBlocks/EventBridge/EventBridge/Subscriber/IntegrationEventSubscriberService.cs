using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBridge.Subscriber;

public abstract class IntegrationEventSubscriberService : IHostedService
{
    private readonly SubscriberEventProcessFunctionStore _subscriberEventProcessFunctionStore;

    protected IntegrationEventSubscriberService(IServiceProvider serviceProvider)
    {
        _subscriberEventProcessFunctionStore = serviceProvider.GetRequiredService<SubscriberEventProcessFunctionStore>();
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