
namespace ecobony.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class HeaderController(IMediator mediator):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] GetHeaderClientAllCommandRequest request)
    {
        GetHeaderClientAllCommandResponse response = await mediator.Send(request);

        return Ok(response);
    }
}