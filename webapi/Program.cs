using System.Reflection;
using FluentValidation;
using Grpc.Core.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;
using webapi;
using webapi.BackgroundJobs;
using webapi.Behaviors;
using webapi.Interceptors;
using webapi.Middlewares;
using webapi.Repositories;
using MessageService = webapi.Services.MessageService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<PostRepository>();

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

builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);

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