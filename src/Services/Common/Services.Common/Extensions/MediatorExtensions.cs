using MediatR;
using Microsoft.EntityFrameworkCore;
using Services.Common.Domain;

namespace Services.Common.Extensions;

public static class MediatorExtensions
{
    public static async Task PublishDomainEventsAsync(this IMediator mediator, DbContext context)
    {
        var domainEntities = context.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        // Clean events
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}