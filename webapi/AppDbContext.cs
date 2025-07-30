using Microsoft.EntityFrameworkCore;
using webapi.Entities;
using webapi.Models;
using webapi.Outbox;

namespace webapi;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<OutboxMessage>  OutboxMessages => Set<OutboxMessage>();
    public DbSet<Blog> Blogs => Set<Blog>();
    public DbSet<Post> Posts => Set<Post>();
}