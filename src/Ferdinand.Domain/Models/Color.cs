using Ferdinand.Domain.Primitives;

namespace Ferdinand.Domain.Models;

public sealed class Color : AggregateRoot<Guid>
{
    public string Tenant { get; private set; }
    public string HexValue { get; private set; } 
    public string Description { get; private set; }
}
