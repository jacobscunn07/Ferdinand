namespace Ferdinand.Domain.Primitives;

public interface IEntity<TKey>
{
    TKey Key { get; }
}
