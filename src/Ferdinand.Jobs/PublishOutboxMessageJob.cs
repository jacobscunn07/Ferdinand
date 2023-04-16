using Ferdinand.Application.Commands.PublishOutboxMessage;
using MediatR;
using Quartz;

namespace Ferdinand.Jobs;

public sealed class PublishOutboxMessageJob : IJob
{
    private readonly IMediator _mediator;
    
    public PublishOutboxMessageJob(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        await _mediator.Send(new PublishOutboxMessageCommand());
    }
}
