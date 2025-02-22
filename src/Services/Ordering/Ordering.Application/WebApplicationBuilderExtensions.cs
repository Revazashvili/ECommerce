using EventBridge.Kafka;
using Microsoft.AspNetCore.Builder;
using Ordering.Application.IntegrationEvents;

namespace Ordering.Application;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication UseApplication(this WebApplication app)
    {
        app.UseKafkaSubscriber(subscriber =>
        {
            subscriber.Subscribe<OrderQuantityNotAvailableIntegrationEvent, OrderQuantityNotAvailableIntegrationEventHandler>("OrderQuantityNotAvailable");
            subscriber.Subscribe<OrderQuantityAvailableIntegrationEvent, OrderQuantityAvailableIntegrationEventHandler>("OrderQuantityAvailable");
            subscriber.Subscribe<OrderPaymentSucceededIntegrationEvent, OrderPaymentSucceededIntegrationEventHandler>("OrderPaymentSucceeded");
            subscriber.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>("OrderPaymentFailed");
        });

        return app;
    }
}