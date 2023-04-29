using Ferdinand.Domain.Models;
using Ferdinand.Infrastructure.Data.Outbox;
using Microsoft.EntityFrameworkCore;

namespace Ferdinand.Infrastructure.EntityFrameworkCore;

public sealed class FerdinandDbContext : DbContext
{
    public FerdinandDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Color> Colors { get; set; } = null!;

    public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyMarker).Assembly);
    }
}
