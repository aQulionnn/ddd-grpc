using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi.Services;

public class MessageService(AppDbContext context) : webapi.MessageService.MessageServiceBase
{
    private readonly AppDbContext _context = context;

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

    public override async Task<MessagesResponse> GetAllMessages(GetAllMessagesRequest request, ServerCallContext context)
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
}