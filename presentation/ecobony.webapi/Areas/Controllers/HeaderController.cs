
namespace ecobony.webapi.Areas.Controllers;

[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class HeaderController(IMediator mediator):ControllerBase
{
    // GET
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, Manager")]
    public async Task<IActionResult> Index([FromQuery] GetAdminAllCommandHandlerRequest request)
    {
        GetAdminAllCommandHandlerResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public async Task<IActionResult> Index([FromRoute] GetByIdCommandRequest request)
    {
        GetByIdCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] CreateHeaderCommandRequest request)
    { CreateHeaderCommandResponse response = await mediator.Send(request);  
        return StatusCode((int)HttpStatusCode.Created);
    }
 
    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Update([FromForm] UpdateHeaderCommandRequest request)
    {
        UpdateHeaderCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("soft-delete/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> SoftDelete([FromRoute] SoftDeleteCommandRequest request)
    { 
        SoftDeleteCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("restore/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Restore([FromRoute] RestoreCommandRequest request)
    { 
        RestoreCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpDelete("delete/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Delete([FromRoute] DeleteCommandRequest request)
    { 
        DeleteCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
}