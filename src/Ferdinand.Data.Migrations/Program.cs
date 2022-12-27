using Ferdinand.Application.Commands.MigrateDatabase;
using Ferdinand.Data.EntityFrameworkCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AssemblyMarker = Ferdinand.Application.AssemblyMarker;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) => 
       {
            var connectionString = ctx.Configuration.GetConnectionString("Postgres");
            services
                .AddDbContext<FerdinandDbContext>(opts => 
                    opts.UseNpgsql(connectionString))
                .AddMediatR(typeof(AssemblyMarker));
       }
    ).Build();

using (var scope = host.Services.CreateScope())
{
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var model = new MigrateDatabaseCommand();
    var result = await mediator.Send(model); 
}
