using MediatR;

namespace webapi.Abstractions.Messaging;

public interface ICommand : IRequest<Unit>
{
    
}

public interface ICommand<TResponse> : IRequest<TResponse>
{
    
}