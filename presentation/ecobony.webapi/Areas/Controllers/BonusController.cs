using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Areas.Controllers;
[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class BonusController(IMediator mediator):ControllerBase
{
   [HttpGet]
    public async Task<IActionResult> Index([FromQuery]GetAdminBonusAllCommandRequest request)
    {
        GetAdminBonusAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("{wasteId}")]
    public async Task<IActionResult> Create([FromRoute]CreateBonusCommandRequest request)
    {
        CreateBonusCommandResponse response = await mediator.Send(request);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut("soft-delete/{Id}")]
    public async Task<IActionResult> SoftDelete([FromRoute] SoftBonusCommandRequest request)
    {
      SoftBonusCommandResponse  response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("restore-delete/{Id}")]
    public async Task<IActionResult> RestoreDelete([FromRoute] RestoreBonusCommandRequest request)
    {
        RestoreBonusCommandResponse  response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpDelete("delete/{Id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteBonusCommandRequest request)
    {
        DeleteBonusCommandResponse  response = await mediator.Send(request);
        return Ok(response);
    }
}