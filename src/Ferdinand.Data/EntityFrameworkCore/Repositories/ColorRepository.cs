using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;

namespace Ferdinand.Data.EntityFrameworkCore.Repositories;

public sealed class ColorRepository : GenericRepository<Color, ColorId>, IColorRepository
{
    public ColorRepository(FerdinandDbContext ctx) : base(ctx)
    {
    }
}
