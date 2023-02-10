namespace Ferdinand.Domain.Primitives;

public interface IAggregateRoot<out TKey> : IEntity<TKey>, IHandleDomainEvents
{
}

// public interface IAggregateRoot<TKey, TTenant> : IAggregateRoot<TKey>
// {
//     TTenant Tenant { get; }
// }
