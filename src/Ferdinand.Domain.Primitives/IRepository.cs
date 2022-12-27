namespace Ferdinand.Domain.Primitives;

public interface IRepository<TAggregate, in TKey>
    where TAggregate : class, IAggregateRoot<TKey>
{
    Task<int> Delete(TKey key);

    Task<bool> Exists(TKey key);

    Task<TAggregate> Get(TKey key);

    Task<TAggregate> Save(TAggregate aggregate);
}
