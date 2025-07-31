using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Polly;
using Quartz;
using SharedKernel.Exceptions;
using SharedKernel.Primitives;

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
            IDomainEvent? domainEvent = null;
            
            try
            {
                domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content);

                if (domainEvent is null)
                    throw new DomainEventDeserializationException("Could not deserialize domain event");
                
                var policy = Policy
                    .Handle<Exception>()
                    .RetryAsync(3);

                await policy.ExecuteAndCaptureAsync(() => 
                    _publisher.Publish(domainEvent, context.CancellationToken));
                
                outboxMessage.ProcessedOn = DateTime.Now;
                outboxMessage.Error = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process OutboxMessage {outboxMessage.Id}");
                outboxMessage.Error = ex.Message; 
            }
        }
        
        await _dbContext.SaveChangesAsync();
    }
}