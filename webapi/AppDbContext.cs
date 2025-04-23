using Microsoft.EntityFrameworkCore;
using webapi.Entities;

namespace webapi;

public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
{
    public DbSet<Message> Messages => Set<Message>();
}