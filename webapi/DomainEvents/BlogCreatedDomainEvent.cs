using webapi.Primitives;

namespace webapi.DomainEvents;

public sealed record BlogCreatedDomainEvent(Guid Id, string Name) : IDomainEvent
{
    
}