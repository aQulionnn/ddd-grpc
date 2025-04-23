using Microsoft.EntityFrameworkCore;
using webapi;
using MessageService = webapi.Services.MessageService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Database"));

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<MessageService>();

app.Run();