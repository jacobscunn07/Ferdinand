using Ferdinand.Domain;
using Ferdinand.Domain.Primitives;
using Throw;

public sealed record Tenant : IValueObject
{
    private Tenant(string value)
    {

        value.Throw(() => new DomainException())
            .IfEmpty()
            .IfWhiteSpace();

        Value = value;
    }

    public string Value { get; set; }

    public static Tenant Create(string value)
    {
        return new Tenant(value);
    }
}
