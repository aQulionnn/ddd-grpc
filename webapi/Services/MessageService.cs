using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi.Services;

public class MessageService(AppDbContext context, ILogger<MessageService> logger)
    : webapi.MessageService.MessageServiceBase
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<MessageService> _logger = logger;

    public override async Task<MessageResponse> CreateMessage(CreateMessageRequest request, ServerCallContext context)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Description = request.Description
        };

        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();

        return new MessageResponse
        {
            Id = message.Id.ToString(),
            Description = message.Description
        };
    }

    public override async Task<MessagesResponse> GetAllMessages(GetAllMessagesRequest request,
        ServerCallContext context)
    {
        var response = new MessagesResponse();
        var messages = await _context.Messages.ToListAsync();

        response.Items.AddRange(messages.Select(message => new MessageResponse
        {
            Id = message.Id.ToString(),
            Description = message.Description,
        }));

        return response;
    }

    public override async Task GetAllMessagesStream(GetAllMessagesRequest request,
        IServerStreamWriter<MessagesResponse> responseStream, ServerCallContext context)
    {
        for (int i = 0; i < 10; i++)
        {
            if (context.CancellationToken.IsCancellationRequested)
            {
                _logger.LogInformation("Request was cancelled");
                break;
            }

            var messages = await _context.Messages.ToListAsync();

            await responseStream.WriteAsync(new MessagesResponse
            {
                Items =
                {
                    messages.Select(message => new MessageResponse
                    {
                        Id = message.Id.ToString(),
                        Description = message.Description,
                    })
                },
                Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
            });
            
            await Task.Delay(1000);
        }
    }
}