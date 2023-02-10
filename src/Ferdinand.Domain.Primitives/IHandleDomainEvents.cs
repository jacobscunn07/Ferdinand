namespace Ferdinand.Domain.Primitives;

public interface IHandleDomainEvents
{
    public IReadOnlyList<IDomainEvent> Events { get; }

    public void RaiseEvent(IDomainEvent evt);
}
