namespace SharedKernel.Exceptions;

public sealed class DomainEventDeserializationException : DomainException
{
    public DomainEventDeserializationException(string message) 
        : base(message)
    {
    }
}