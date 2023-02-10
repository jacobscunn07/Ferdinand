using Ferdinand.Domain.Primitives;

namespace Ferdinand.Domain.Events;

public sealed record ColorAdded(string Tenant, string HexValue) : IDomainEvent;
