using System.Linq.Expressions;

namespace Ferdinand.Domain.Primitives;

public interface IRepository<TAggregate, in TKey>
    where TAggregate : class, IAggregateRoot<TKey>
{
    Task<bool> Exists(TKey key);
    Task<TAggregate?> GetByKey(TKey key);
    IEnumerable<TAggregate> Find(Expression<Func<TAggregate, bool>> expression);
    Task Add(TAggregate aggregate);
    Task AddRange(IEnumerable<TAggregate> aggregates);
    Task Remove(TAggregate aggregate);
    Task RemoveRange(IEnumerable<TAggregate> aggregates);
}
