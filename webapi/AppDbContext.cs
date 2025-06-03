using Microsoft.EntityFrameworkCore;
using webapi.Entities;
using webapi.Outbox;

namespace webapi;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<OutboxMessage>  OutboxMessages => Set<OutboxMessage>();
}