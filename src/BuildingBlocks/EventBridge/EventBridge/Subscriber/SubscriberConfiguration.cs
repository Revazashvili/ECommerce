using System.Reflection;

namespace EventBridge.Subscriber;

public class SubscriberConfiguration
{
    internal List<Assembly> AssembliesToRegister { get; } = [];
    internal Action<IIntegrationEventSubscriber> Subscriber { get; private set; }

    public void RegisterServicesFromAssembly(params Assembly[] assemblies)
    {
        AssembliesToRegister.AddRange(assemblies);
    }

    public void ConfigureSubscriber(Action<IIntegrationEventSubscriber> subscriber)
    {
        Subscriber = subscriber;
    }
}