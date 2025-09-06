
namespace ecobony.webapi.Areas.Controllers;
[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class LanguageController(IMediator _mediator):ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll([FromQuery] GetAdminAllCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public async Task<IActionResult> GetById([FromRoute] GetByIdCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] CreateLanguageCommandRequest request)
    { 
        CreateLanguageCommandResponse response = await _mediator.Send(request);
        return StatusCode((int)HttpStatusCode.Created);
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Update([FromForm] UpdateLanguageCommandRequest request)
    {
        UpdateLanguageCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("soft-delete/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> SoftDelete([FromRoute] SoftDeleteCommandRequest request)
    {
        SoftDeleteCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("restore/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Restore([FromRoute]RestoreCommandRequest request)
    {
        RestoreCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("delete/{Id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> Delete(DeleteCommandRequest request)
    {
        DeleteCommandResponse response = await _mediator.Send(request);
        return Ok(response);
    }
}