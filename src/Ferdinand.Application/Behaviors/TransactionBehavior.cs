using Ferdinand.Data.EntityFrameworkCore;
using MediatR;

namespace Ferdinand.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly FerdinandDbContext _ctx;

    public TransactionBehavior(FerdinandDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await using var tx = await _ctx.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var result = await next();

            await _ctx.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
            
            return result;
        }
        catch
        {
            await tx.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
