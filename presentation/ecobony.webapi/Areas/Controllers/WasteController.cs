using ecobony.application.Feauters.Query;
using ecobony.application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Areas.Controllers;

[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class WasteController(IMediator mediator):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] GetAdminWasteAllCommandRequest request)
    {
        GetAdminWasteAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
}