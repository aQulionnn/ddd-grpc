namespace webapi.Exceptions;

public sealed class DomainEventDeserializationException : DomainException
{
    public DomainEventDeserializationException(string message) 
        : base(message)
    {
    }
}