using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;

namespace ecobony.webapi.Areas.Controllers;
[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class UserController(IMediator mediator):ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public async Task<IActionResult> Index([FromQuery]GetAllUsersAsyncCommandRequest request)
    {
        GetAllUsersAsyncCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("delete/{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> AssignRoleDeleteUser([FromRoute]AssignRoleDeleteUserCommandRequest request)
    {
        AssignRoleDeleteUserCommandResponse response =await mediator.Send(request);
        return Ok(response); 
    }

    [HttpPost("assign-role-to-user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleToUserAsyncCommandRequest request)
    {
        AssignRoleToUserAsyncCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
}