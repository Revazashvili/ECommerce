using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.IntegrationEvents.EventHandlers;
using Ordering.Application.IntegrationEvents.Events;

namespace Ordering.Application;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication SubscribeToEvents(this WebApplication app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        eventBus.SubscribeAsync<OrderQuantityNotAvailableIntegrationEvent, OrderQuantityNotAvailableIntegrationEventHandler>();
        eventBus.SubscribeAsync<OrderQuantityAvailableIntegrationEvent, OrderQuantityAvailableIntegrationEventHandler>();

        return app;
    }
}