using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator):ControllerBase
{
   [HttpGet]
    public async Task<IActionResult> Index([FromQuery]UserDashboardCommandRequest request)
    {
        UserDashboardCommandResponse response = await mediator.Send(request);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest request)
    {
        UpdatePasswordCommandResponse response = await mediator.Send(request);
        return StatusCode((int)HttpStatusCode.Created);
    }
}