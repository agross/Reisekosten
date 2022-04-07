using Application.UseCases.Erfassen;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ErfassenController : ControllerBase
{
  readonly IMediator _mediator;

  public ErfassenController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpPost("")]
  public async Task<ActionResult> Post([FromBody] ErfassenCommand command)
  {
    await _mediator.Send(command);

    return Accepted();
  }
}
