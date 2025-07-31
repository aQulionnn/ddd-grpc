using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;
using SharedKernel.Primitives;

namespace webapi.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessagesInterceptor 
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        DbContext? dbContext = eventData.Context;
        
        if (dbContext is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(e => e.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents();

                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOn = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                })
            })
            .ToList();
        
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}