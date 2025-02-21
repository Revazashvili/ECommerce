// using EventBus;
// using MediatR;
// using Microsoft.Extensions.Logging;
// using Ordering.Application.Features.SetOrderPaidStatus;
// using Ordering.Application.IntegrationEvents.Events;
//
// namespace Ordering.Application.IntegrationEvents.EventHandlers;
//
// public class OrderPaymentSucceededIntegrationEventHandler(ILogger<OrderPaymentSucceededIntegrationEventHandler> logger,
//         ISender sender)
//     : IIntegrationEventHandler<OrderPaymentSucceededIntegrationEvent>
// {
//     public async Task Handle(OrderPaymentSucceededIntegrationEvent @event)
//     {
//         try
//         {
//             await sender.Send(new SetOrderPaidStatusCommand(@event.OrderNumber));
//         }
//         catch (Exception exception)
//         {
//             logger.LogError(exception, "Error occured in {Handler}",
//                 nameof(OrderPaymentFailedIntegrationEventHandler));
//         }
//     }
// }