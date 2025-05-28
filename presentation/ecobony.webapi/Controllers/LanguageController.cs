

namespace ecobony.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class LanguageController(IMediator mediator):ControllerBase
{
   [HttpGet]
    public async Task<IActionResult> Index([FromQuery]  GetClientAllCommandRequest request )
    {
        GetClientAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("filter")]
    public async Task<IActionResult> Index([FromQuery] GetByIsoCodeCommandRequest request)
    {
        GetByIsoCodeCommandResponse response  = await mediator.Send(request);
        return Ok(response);
    }
}