namespace Domain.Entities;

public class Message
{
    public Guid Id { get; set; }
    public required string Description { get; set; }
}