using System.Reflection;
using Grpc.Core.Interceptors;
using Microsoft.EntityFrameworkCore;
using Quartz;
using webapi;
using webapi.BackgroundJobs;
using webapi.Interceptors;
using webapi.Middlewares;
using MessageService = webapi.Services.MessageService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

builder.Services.AddQuartz(configure =>
{
    var jobKey = new  JobKey(nameof(ProcessOutboxMessagesJob));
    
    configure
        .AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(trigger => trigger
            .ForJob(jobKey)
            .WithSimpleSchedule(schedule => schedule
                .WithIntervalInSeconds(10)
                .RepeatForever()));
});

builder.Services.AddQuartzHostedService();

builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

builder.Services.AddDbContext<AppDbContext>((provider, options) =>
{
    options.UseInMemoryDatabase("Database")
        .AddInterceptors(provider.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>());
});

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapGrpcReflectionService();
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapGrpcService<MessageService>();

app.Run();