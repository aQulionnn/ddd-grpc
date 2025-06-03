using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using webapi.Primitives;

namespace webapi.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(AppDbContext dbContext, IPublisher publisher, ILogger<ProcessOutboxMessagesJob> logger) 
    : IJob
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly IPublisher _publisher = publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger = logger;
    
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext.OutboxMessages
            .Where(x => x.ProcessedOn == null)
            .Take(10)
            .ToListAsync(context.CancellationToken);

        foreach (var outboxMessage in messages)
        {
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content);

            if (domainEvent is null)
            {
                _logger.LogError($"{outboxMessage.Id} Domain event is null");
                continue;
            }
            
            await _publisher.Publish(domainEvent, context.CancellationToken);
            outboxMessage.ProcessedOn = DateTime.Now;
        }
        
        await _dbContext.SaveChangesAsync();
    }
}