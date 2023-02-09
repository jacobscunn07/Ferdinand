namespace Ferdinand.Data.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime? ProcessedUtc { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
}
