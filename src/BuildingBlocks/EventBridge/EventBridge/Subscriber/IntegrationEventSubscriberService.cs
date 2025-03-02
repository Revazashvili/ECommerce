using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EventBridge.Subscriber;

public abstract class IntegrationEventSubscriberService : IHostedService
{
    private readonly SubscriberEventProcessFunctionStore _subscriberEventProcessFunctionStore;
    private CancellationTokenSource _stoppingCts;

    protected IntegrationEventSubscriberService(IServiceProvider serviceProvider)
    {
        _subscriberEventProcessFunctionStore = serviceProvider.GetRequiredService<SubscriberEventProcessFunctionStore>();
    }

    protected abstract Task Subscribe(string topic, ProcessEvent processEventFunction, CancellationToken cancellationToken);
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var processEvents = _subscriberEventProcessFunctionStore.GetProcessEventFunctions();

        _stoppingCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        
        foreach (var processEvent in processEvents)
            Task.Run(() => Subscribe(processEvent.Key, processEvent.Value, _stoppingCts.Token), _stoppingCts.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _stoppingCts.CancelAsync();
    }
}