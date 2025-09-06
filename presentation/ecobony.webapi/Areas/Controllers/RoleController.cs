
using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;

namespace ecobony.webapi.Areas.Controllers;
[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class RoleController(IMediator mediator):ControllerBase
{
   [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public async Task<IActionResult> Index([FromQuery]GetRoleAllCommandRequest request)
    {
        GetRoleAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("(get-by-id)/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public async Task<IActionResult> Index([FromQuery]GetRoleByIdCommandRequest request)
    {
        GetRoleByIdCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody]CreateRoleCommandRequest request)
    {
        CreateRoleCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Update([FromBody]UpdateRoleCommandRequest request)
    {
        UpdateRoleCommandResponse response = await mediator.Send(request);
        
        return Ok(response);
    }
    [HttpDelete("delete/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute]DeleteRoleCommandRequest request)
    {
        DeleteRoleCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
}