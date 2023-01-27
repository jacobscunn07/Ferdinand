using Ferdinand.Domain.Primitives;

public record ColorId : IValueObject
{
    private ColorId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; set; }

    public static ColorId Create(Guid value)
    {
        return new ColorId(value);
    }

    public static ColorId CreateUnique()
    {
        return new ColorId(Guid.NewGuid());
    }
}
