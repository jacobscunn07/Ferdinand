using Ferdinand.Api;
using Ferdinand.Application;
using Ferdinand.Data;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseSerilog((hostContext, _, configuration) =>
{
    configuration
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.WithDemystifiedStackTraces();

    if (!hostContext.HostingEnvironment.IsDevelopment())
    {
        configuration.WriteTo.Console(new RenderedCompactJsonFormatter());
    }
    else
    {
        configuration.WriteTo.Console();
    }
});

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDataServices(builder.Configuration)
    .AddApplicationServices()
    .AddMiddleware();

var app = builder.Build();

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
