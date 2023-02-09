using Ferdinand.Application;
using Ferdinand.Data;
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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDataServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
