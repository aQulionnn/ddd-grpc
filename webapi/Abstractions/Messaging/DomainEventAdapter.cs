using MediatR;
using SharedKernel.Primitives;

namespace webapi.Abstractions.Messaging;

public sealed class DomainEventAdapter<TDomainEvent>(TDomainEvent domainEvent) : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}