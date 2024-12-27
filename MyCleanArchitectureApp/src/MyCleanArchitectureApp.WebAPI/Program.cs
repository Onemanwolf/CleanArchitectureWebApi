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


var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyCleanArchitectureApp API", Version = "v1" });
});

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<OrderProfile>());

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

app.MapControllers();

app.Run();