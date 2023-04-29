using Ferdinand.Infrastructure.Data.Outbox;
using Ferdinand.Infrastructure.EntityFrameworkCore;

namespace Ferdinand.Infrastructure.Data.EntityFrameworkCore.Repositories;

public class OutboxMessageRepository
{
    private readonly FerdinandDbContext _ctx;

    public OutboxMessageRepository(FerdinandDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task AddRange(IEnumerable<OutboxMessage> entities)
    {
        await _ctx.OutboxMessages.AddRangeAsync(entities);
    }

    public IEnumerable<OutboxMessage> GetAll()
    {
        return _ctx.OutboxMessages.AsEnumerable();
    }
}
