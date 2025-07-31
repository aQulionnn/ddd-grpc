using SharedKernel.Primitives;

namespace Domain.DomainEvents;

public sealed record BlogCreatedDomainEvent(Guid Id, string Name) : IDomainEvent
{
    
}