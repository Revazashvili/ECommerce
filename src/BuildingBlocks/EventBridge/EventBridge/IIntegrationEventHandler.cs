namespace EventBridge;

public interface IIntegrationEventHandler<in TEvent>
    where TEvent : IntegrationEvent
{
    Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
}

public class TestEvent : IntegrationEvent
{
    protected TestEvent(object aggregateId)
    {
        AggregateId = aggregateId.ToString();
    }
}

public class Test : IIntegrationEventHandler<TestEvent>
{
    public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}