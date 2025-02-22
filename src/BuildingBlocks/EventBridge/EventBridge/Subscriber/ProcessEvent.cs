namespace EventBridge.Subscriber;

public delegate Task ProcessEvent(string payload, IServiceProvider serviceProvider, CancellationToken cancellationToken);