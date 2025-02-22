using EventBridge.Kafka;
using Microsoft.AspNetCore.Builder;
using Products.Application.IntegrationEvents;

namespace Products.Application;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication SubscribeToEvents(this WebApplication app)
    {
        app.UseKafkaSubscriber(subscriber =>
        {
            subscriber.Subscribe<SetOrderPendingStatusIntegrationEvent, SetOrderPendingStatusIntegrationEventHandler>("OrderSetOrderPendingStatus");
        });

        return app;
    }
}