using EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Products.Application;

public static class WebApplicationBuilderExtensions
{
    public static WebApplication SubscribeToEvents(this WebApplication app)
    {
        var eventBus = app.Services.GetRequiredService<IEventBus>();
        // eventBus.SubscribeAsync<SetOrderPendingStatusIntegrationEvent, SetOrderPendingStatusIntegrationEventHandler>();

        return app;
    }
}