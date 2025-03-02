using System.Reflection;

namespace EventBridge.Subscriber;

public class EventBridgeSubscriberConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    
    public Action<IIntegrationEventSubscriber> Subscriber { get; set; }

    public EventBridgeSubscriberConfiguration RegisterServicesFromAssembly(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
        return this;
    }
}