using Ferdinand.Application;
using Ferdinand.Application.Commands.MigrateDatabase;
using Ferdinand.Data;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) => 
       {
            services
                .AddDataServices(ctx.Configuration)
                .AddApplicationServices();
       }
    ).Build();

using (var scope = host.Services.CreateScope())
{
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var model = new MigrateDatabaseCommand();
    var result = await mediator.Send(model); 
}
