using Ferdinand.Domain.Primitives;

namespace Ferdinand.Domain.Models;

public record ColorKey : IValueObject
{
    private ColorKey(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; set; }

    public static ColorKey Create(Guid value)
    {
        return new ColorKey(value);
    }

    public static ColorKey CreateUnique()
    {
        return new ColorKey(Guid.NewGuid());
    }
}