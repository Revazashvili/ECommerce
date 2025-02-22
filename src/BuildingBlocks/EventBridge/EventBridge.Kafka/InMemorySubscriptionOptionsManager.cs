namespace EventBridge.Kafka;

internal class InMemorySubscriptionOptionsManager
{
    private readonly Dictionary<string, ProcessEvent> _processEvents = [];
    internal void AddProcessEvent(string topic, ProcessEvent processEvent)
    {
        if (_processEvents.Any(options => options.Key == topic))
            return;
        
        _processEvents[topic] = processEvent;
    }
    
    public Dictionary<string, ProcessEvent> GetProcessEvents() => _processEvents;
}