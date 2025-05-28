
using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;

namespace ecobony.webapi.Areas.Controllers;
[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class RoleController(IMediator mediator):ControllerBase
{
   [HttpGet]
    public async Task<IActionResult> Index([FromQuery]GetRoleAllCommandRequest request)
    {
        GetRoleAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> Index([FromQuery]GetRoleByIdCommandRequest request)
    {
        GetRoleByIdCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]CreateRoleCommandRequest request)
    {
        CreateRoleCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Create([FromBody]UpdateRoleCommandRequest request)
    {
        UpdateRoleCommandResponse response = await mediator.Send(request);
        
        return Ok(response);
    }
    [HttpDelete("delete/{Id}")]
    public async Task<IActionResult> Delete([FromRoute]DeleteRoleCommandRequest request)
    {
        DeleteRoleCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
}