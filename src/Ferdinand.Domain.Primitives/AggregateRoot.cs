namespace Ferdinand.Domain.Primitives;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
where TKey : IValueObject
{
    private IList<IDomainEvent> _events;

    protected AggregateRoot()
    {
        _events = new List<IDomainEvent>();
    }

    public IReadOnlyList<IDomainEvent> Events => _events.AsReadOnly();

    public void RaiseEvent(IDomainEvent evt)
    {
        _events.Add(evt);
    }
}
