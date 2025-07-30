using MediatR;
using webapi.DomainEvents;

namespace webapi.Features.Blogs.Events;

internal sealed class BlogCreatedDomainEventHandler(ILogger<BlogCreatedDomainEventHandler> logger)
    : INotificationHandler<BlogCreatedDomainEvent>
{
    private readonly ILogger<BlogCreatedDomainEventHandler> _logger = logger;

    public Task Handle(BlogCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"{notification.Name} blogEntity created.");
        return Task.CompletedTask;;
    }
}