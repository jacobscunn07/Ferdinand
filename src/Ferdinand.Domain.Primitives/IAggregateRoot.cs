namespace Ferdinand.Domain.Primitives;

public interface IAggregateRoot<TKey> : IEntity<TKey>
{
    IReadOnlyList<IDomainEvent> Events { get; }

    void RaiseEvent(IDomainEvent evt);
}

// public interface IAggregateRoot<TKey, TTenant> : IAggregateRoot<TKey>
// {
//     TTenant Tenant { get; }
// }
