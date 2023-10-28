namespace EventBus;

public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<Type>> _eventSubscriptions = new();
    
    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
           
        //create a new event in eventSubscription if not exists
        if (!HasEvent<T>())
            _eventSubscriptions.Add(eventName, new List<Type>());

        if (_eventSubscriptions[eventName].Any(si => si is TH))
            throw new ArgumentException($"HandlerType {typeof(TH).Name} is already registered");

        _eventSubscriptions[eventName].Add(typeof(TH));
    }

    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent => _eventSubscriptions[GetEventKey<T>()];

    public bool HasEvent<T>()
        where T : IntegrationEvent 
        => _eventSubscriptions.ContainsKey(GetEventKey<T>());

    public void RemoveSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
        var handlerToRemove = _eventSubscriptions[eventName].SingleOrDefault(si => si is TH)!;
        _eventSubscriptions[eventName].Remove(handlerToRemove);
    }
    
    public void Clear() => _eventSubscriptions.Clear();
    public bool IsEmpty => !_eventSubscriptions.Any();

    private static string GetEventKey<T>() => typeof(T).Name;
}