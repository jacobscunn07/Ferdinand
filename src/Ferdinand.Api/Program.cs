using Ferdinand.Api;
using Ferdinand.Application;
using Ferdinand.Extensions.Hosting;
using Ferdinand.Infrastructure;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using Ferdinand.Infrastructure.Logging;
using Ferdinand.Infrastructure.Messaging;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;
using AssemblyMarker = Ferdinand.Api.AssemblyMarker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>));
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Host.ConfigureNServiceBus(typeof(AssemblyMarker).Assembly.GetName().Name!);
builder.Services.AddSingleton<IMessageBus, NServiceBusMessageBus>();

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.ReportApiVersions = true;
    o.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader());
});

builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDataServices(builder.Configuration)
    .AddApplicationServices()
    .AddMiddleware();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<FerdinandDbContext>();

var app = builder.Build();

app.MapHealthChecks("/readyz", new HealthCheckOptions
{
    Predicate = _ => true
});

app.MapHealthChecks("/livez", new HealthCheckOptions
{
    Predicate = _ => false
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
