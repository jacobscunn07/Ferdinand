using Ferdinand.Domain.Primitives;

namespace Ferdinand.Domain.Models;

public sealed record ColorKey(Guid Value) : IValueObject
{
    public static ColorKey Create(string value)
    {
        if (Guid.TryParse(value, out var guidOutput))
        {
            return Create(guidOutput);
        }

        throw new DomainException();
    }

    public static ColorKey Create(Guid value)
    {
        return new ColorKey(value);
    }

    public static ColorKey CreateUnique()
    {
        return new ColorKey(Guid.NewGuid());
    }
}
