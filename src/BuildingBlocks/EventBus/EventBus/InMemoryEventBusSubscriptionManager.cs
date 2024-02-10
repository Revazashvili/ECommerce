namespace EventBus;

public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
{
    private readonly Dictionary<string, List<Type>> _eventSubscriptions = new();
    
    ///<inheritdoc cref="IEventBusSubscriptionManager.AddSubscription{T,TH}"/>
    public void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = GetEventKey<T>();
           
        if (!_eventSubscriptions.ContainsKey(GetEventKey<T>()))
            _eventSubscriptions.Add(eventName, new List<Type>());

        if (_eventSubscriptions[eventName].Any(si => si is TH))
            throw new ArgumentException($"HandlerType {typeof(TH).Name} is already registered");

        _eventSubscriptions[eventName].Add(typeof(TH));
    }

    ///<inheritdoc cref="IEventBusSubscriptionManager.GetHandlersForEvent{T}"/>
    public IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent => _eventSubscriptions[GetEventKey<T>()];
    private static string GetEventKey<T>() => typeof(T).Name;
}