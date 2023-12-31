using System.Text;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace EventBus.Kafka;

public class KafkaEventBus : IEventBus
{
    private readonly IEventBusSubscriptionManager _subscriptionManager;
    private readonly ILogger<KafkaEventBus> _logger;
    private readonly KafkaConnection _kafkaConnection;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public KafkaEventBus(IEventBusSubscriptionManager subscriptionManager, ILogger<KafkaEventBus> logger,
        KafkaConnection kafkaConnection, IServiceProvider serviceProvider)
    {
        _subscriptionManager = subscriptionManager ?? throw new ArgumentNullException(nameof(subscriptionManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _kafkaConnection = kafkaConnection ?? throw new ArgumentNullException(nameof(kafkaConnection));
        _serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                               ?? throw new ArgumentException($"Cannot resolve IServiceScopeFactory from {nameof(serviceProvider)}");
    }
    
    public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
    {
        var eventType = typeof(T).Name;

        try
        {
            using var producer = _kafkaConnection.BuildProducer();
            var json = JsonSerializer.Serialize(@event);
            var producerResult = await producer.ProduceAsync(eventType, new Message<Null, string> { Value = json });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occured during publishing the event to topic {eventType}");
            _logger.LogError(ex.Message + "\n" + ex.StackTrace);
        }
    }

    public async Task SubscribeAsync<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        using var consumer = _kafkaConnection.BuildConsumer();
        
        _subscriptionManager.AddSubscription<T, TH>();
        consumer.Subscribe(eventName);
        
        //create a task to listen to the topic
        await Task.Run(async () =>
        {
            while (true)
            {
                try
                {
                    var consumerResult = consumer.Consume();
                    var value = JsonSerializer.Deserialize<T>(consumerResult.Message.Value)!;
                    
                    _logger.LogInformation("Started processing Event: {EventName} With Data: {@EventData}",eventName,value);
                    await ProcessEvent(value);
                    _logger.LogInformation("Finished processing Event: {EventName} With Data: {@EventData}",eventName,value);
                }
                catch (ConsumeException e)
                {
                    _logger.LogError($"Error `{e.Error.Reason}` occured during consuming the event from topic {eventName}");
                    _logger.LogError(e.Message + "\n" + e.StackTrace);
                }
            }
        });
    }

    private async Task ProcessEvent<T>(T value) where T : IntegrationEvent
    {
        if (!_subscriptionManager.HasEvent<T>()) 
            return;

        using var scope = _serviceScopeFactory.CreateScope();
        
        var subscriptions = _subscriptionManager.GetHandlersForEvent<T>();
        foreach (var subscription in subscriptions)
        {
            var handler = scope.ServiceProvider.GetRequiredService(subscription);
            if (handler == null)
                continue;
                    
            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));
            await (Task)concreteType.GetMethod("Handle")!
                .Invoke(handler, new object[] { value })!;
        }
    }
}