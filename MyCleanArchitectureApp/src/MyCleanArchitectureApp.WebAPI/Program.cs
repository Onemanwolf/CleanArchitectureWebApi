using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCleanArchitectureApp.Application.Interfaces;
using MyCleanArchitectureApp.Infrastructure.Data;
using MyCleanArchitectureApp.Infrastructure.Messaging;
using MyCleanArchitectureApp.Infrastructure.Interfaces;
using MyCleanArchitectureApp.Domain.Interfaces;
using MyCleanArchitectureApp.Application.Services;
using Microsoft.Azure.Storage.Queue;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Queues;
using Microsoft.OpenApi.Models;
using AutoMapper;
using MyCleanArchitectureApp.Application.MappingProfiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using MyCleanArchitectureApp.Application.Validators;

using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Diagnostics;
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    // dotnet add package Serilog.Sinks.File
    // .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();





var builder = WebApplication.CreateBuilder(args);
try
{
    Log.Information("Starting web application");

    builder.Host.UseSerilog(); // TODO: Move this line to after 'var builder = ...' line

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
var azureQueueSettings = builder.Configuration.GetSection("AzureQueueSettings");
builder.Services.AddSingleton<IAzureQueueService>(sp =>
{
    var queueClient = new QueueClient(azureQueueSettings["ConnectionString"], azureQueueSettings["QueueName"]);
    return new AzureQueueService(queueClient);
});
builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<OrderValidator>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCleanArchitectureApp API", Version = "v1" });
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<OrderProfile>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddHealthChecks();

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .WriteTo.Console());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyCleanArchitectureApp.WebAPI v1");
        c.RoutePrefix = string.Empty; // This will make Swagger UI available at the root URL
    });
}
// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            await context.Response.WriteAsync(new
            {
                context.Response.StatusCode,
                Message = "Internal Server Error."
            }.ToString());
        }
    });
});

app.MapControllers();

app.Run();