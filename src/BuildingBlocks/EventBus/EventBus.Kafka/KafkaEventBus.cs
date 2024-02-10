using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EventBus.Kafka;

public class KafkaEventBus(
        IEventBusSubscriptionManager subscriptionManager,
        ILogger logger,
        KafkaConnection kafkaConnection,
        IServiceProvider serviceProvider)
    : IEventBus
{
    ///<inheritdoc cref="IEventBus.PublishAsync{T}"/>
    public async Task PublishAsync<T>(T @event) where T : IntegrationEvent
    {
        var eventType = typeof(T).Name;

        try
        {
            using var producer = kafkaConnection.BuildProducer();
            var json = JsonSerializer.Serialize(@event);
            _ = await producer.ProduceAsync(eventType, new Message<Null, string> { Value = json });
        }
        catch (Exception ex)
        {
            logger.LogError($"Error occured during publishing the event to topic {eventType}");
            logger.LogError(ex.Message + "\n" + ex.StackTrace);
        }
    }

    ///<inheritdoc cref="IEventBus.SubscribeAsync{T,TH}"/>
    public async Task SubscribeAsync<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        var eventName = typeof(T).Name;
        using var consumer = kafkaConnection.BuildConsumer();

        subscriptionManager.AddSubscription<T, TH>();
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

                    logger.LogInformation("Started processing Event: {EventName} With Data: {@EventData}", 
                        eventName,value);
                    
                    await ProcessEvent(value);
                    
                    logger.LogInformation("Finished processing Event: {EventName} With Data: {@EventData}", 
                        eventName,value);
                }
                catch (ConsumeException e)
                {
                    logger.LogError(
                        $"Error `{e.Error.Reason}` occured during consuming the event from topic {eventName}");
                    logger.LogError(e.Message + "\n" + e.StackTrace);
                }
            }
        });
    }

    private async Task ProcessEvent<T>(T value) where T : IntegrationEvent
    {
        var subscriptions = subscriptionManager.GetHandlersForEvent<T>();
        foreach (var subscription in subscriptions)
        {
            var handler = serviceProvider.GetRequiredService(subscription);

            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(typeof(T));
            await (Task)concreteType.GetMethod("Handle")!
                .Invoke(handler, new object[] { value })!;
        }
    }
}