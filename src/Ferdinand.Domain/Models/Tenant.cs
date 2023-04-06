using Ferdinand.Domain.Primitives;
using Throw;

namespace Ferdinand.Domain.Models;

public sealed record Tenant : IValueObject
{
    private Tenant(string value)
    {
        value.Throw(() => new DomainException())
            .IfEmpty()
            .IfWhiteSpace();

        Value = value;
    }

    public string Value { get; }

    public static Tenant Create(string value)
    {
        return new Tenant(value);
    }
}