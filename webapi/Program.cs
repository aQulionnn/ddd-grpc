using System.Reflection;
using Microsoft.EntityFrameworkCore;
using webapi;
using MessageService = webapi.Services.MessageService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Database"));

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.MapGrpcService<MessageService>();

app.Run();