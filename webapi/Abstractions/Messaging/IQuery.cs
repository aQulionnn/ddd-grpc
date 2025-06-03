using MediatR;

namespace webapi.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<TResponse>
{
    
}