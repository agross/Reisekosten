using Application.UseCases.Bericht;

using Domain.Entities;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuswertungController : ControllerBase
{
  readonly IMediator _mediator;

  public AuswertungController(IMediator mediator)
  {
    _mediator = mediator;
  }

  [HttpGet("")]
  public async Task<ActionResult<BerichtViewModel>> Get()
  {
    var query = new ErzeugeBerichtQuery();

    var result = await _mediator.Send(query);

    return result;
  }
}
