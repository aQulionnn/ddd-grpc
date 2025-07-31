using Domain.DomainEvents;
using MediatR;
using webapi.Abstractions.Messaging;

namespace webapi.Features.Blogs.Events;

internal sealed class BlogCreatedDomainEventHandler(ILogger<BlogCreatedDomainEventHandler> logger)
    : INotificationHandler<DomainEventAdapter<BlogCreatedDomainEvent>>
{
    private readonly ILogger<BlogCreatedDomainEventHandler> _logger = logger;

    public Task Handle(DomainEventAdapter<BlogCreatedDomainEvent> notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{notification.DomainEvent.Name} blogEntity created.");
        return Task.CompletedTask;;
    }
}