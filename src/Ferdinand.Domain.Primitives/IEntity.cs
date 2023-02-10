namespace Ferdinand.Domain.Primitives;

public interface IEntity<out TKey>
{
    TKey Key { get; }
}
