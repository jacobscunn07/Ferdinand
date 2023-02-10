using Ferdinand.Application.Commands.AddColor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ferdinand.Api.Controllers;

[ApiController]
[Route("api/{tenant}/[controller]")]
public class ColorsController : ControllerBase
{
    private readonly ILogger<ColorsController> _logger;
    private readonly IMediator _mediator;

    public ColorsController(ILogger<ColorsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<StatusCodeResult> Post([FromRoute] string tenant, [FromBody] AddColorRequest req)
    {
        var command = new AddColorCommand(tenant, req.HexValue, req.Description);
        await _mediator.Send(command);
        
        return Ok();
    }
}
