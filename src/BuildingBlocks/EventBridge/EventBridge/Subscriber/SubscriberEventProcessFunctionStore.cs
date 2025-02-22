namespace EventBridge.Subscriber;

internal class SubscriberEventProcessFunctionStore
{
    private readonly Dictionary<string, ProcessEvent> _processEvents = [];
    internal void AddProcessEventFunction(string topic, ProcessEvent processEvent)
    {
        if (_processEvents.Any(options => options.Key == topic))
            return;
        
        _processEvents[topic] = processEvent;
    }
    
    public Dictionary<string, ProcessEvent> GetProcessEventFunctions() => _processEvents;
}