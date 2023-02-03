using System.Linq.Expressions;
using Ferdinand.Domain.Primitives;
using Microsoft.EntityFrameworkCore;

namespace Ferdinand.Data.EntityFrameworkCore.Repositories;

public abstract class GenericRepository<T, TKey> : IRepository<T, TKey>
where T : class, IAggregateRoot<TKey>
{
    private readonly FerdinandDbContext _ctx;

    protected GenericRepository(FerdinandDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<bool> Exists(TKey key)
    {
        return await _ctx.Set<T>().AnyAsync(x => x.Key!.Equals(key));
    }

    public async Task Add(T entity)
    {
        await _ctx.Set<T>().AddAsync(entity);
    }

    public async Task AddRange(IEnumerable<T> entities)
    {
        await _ctx.Set<T>().AddRangeAsync(entities);
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _ctx.Set<T>().Where(expression);
    }

    public async Task<T?> GetByKey(TKey key)
    {
        return await _ctx.Set<T>().FindAsync(key);
    }

    public Task Remove(T entity)
    {
        _ctx.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task RemoveRange(IEnumerable<T> entities)
    {
        _ctx.Set<T>().RemoveRange(entities);
        return Task.CompletedTask;
    }
}
