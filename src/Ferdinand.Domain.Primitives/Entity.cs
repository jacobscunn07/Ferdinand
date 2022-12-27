namespace Ferdinand.Domain.Primitives;

public abstract class Entity<TKey> : IEquatable<IEntity<TKey>>, IEntity<TKey>
{
    public TKey Key { get; protected set; }

    public bool Equals(IEntity<TKey> other)
    {
        return EqualityComparer<TKey>.Default.Equals(Key, other.Key);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Entity<TKey>)obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TKey>.Default.GetHashCode(Key);
    }

    public static bool operator ==(Entity<TKey>? left, Entity<TKey>? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TKey>? left, Entity<TKey>? right)
    {
        return !Equals(left, right);
    }
}
