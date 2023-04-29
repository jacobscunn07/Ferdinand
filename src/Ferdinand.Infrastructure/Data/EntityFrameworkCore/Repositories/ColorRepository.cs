using System.Linq.Expressions;
using Ferdinand.Domain.Models;
using Ferdinand.Domain.Repositories;
using Ferdinand.Infrastructure.EntityFrameworkCore;
using Ferdinand.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Ferdinand.Infrastructure.Data.EntityFrameworkCore.Repositories;

public sealed class ColorRepository : IColorRepository
{
    private readonly FerdinandDbContext _ctx;

    public ColorRepository(FerdinandDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<bool> Exists(ColorKey key)
    {
        return await _ctx.Colors.AnyAsync(x => x.Key.Equals(key));
    }

    public async Task Add(Color color)
    {
        await _ctx.Colors.AddAsync(color);
    }

    public async Task AddRange(IEnumerable<Color> colors)
    {
        await _ctx.Colors.AddRangeAsync(colors);
    }

    public IEnumerable<Color> Find(Expression<Func<Color, bool>> expression)
    {
        return _ctx.Colors.Where(expression);
    }

    public async Task<Color> GetByKey(ColorKey key)
    {
        var color = await _ctx.Colors.FindAsync(key);

        if (color is null)
        {
            throw new DataException($"Failed to find color with id {key}");
        }
        
        return color;
    }

    public Task Remove(Color color)
    {
        _ctx.Colors.Remove(color);
        return Task.CompletedTask;
    }

    public Task RemoveRange(IEnumerable<Color> colors)
    {
        _ctx.Colors.RemoveRange(colors);
        return Task.CompletedTask;
    }
}
