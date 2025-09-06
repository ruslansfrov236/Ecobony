using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BonusController(IMediator mediator):ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager, User")]
    public async Task<IActionResult> Index([FromQuery]GetClientBonusAllCommandRequest request)
    {
        GetClientBonusAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("{wasteId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager, User")]
    public async Task<IActionResult> Create([FromBody] CreateBonusCommandRequest request)
    {
        CreateBonusCommandResponse  response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("get-bonus-pdf-view")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager,User")]
    public async Task<IActionResult> GetBonusPdfView([FromQuery] GetClientPdfViewCommandRequest request)
    {
        GetClientPdfViewCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("get-bonus-excel-view")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager,User")]
    public async Task<IActionResult> GetBonusPdfView([FromQuery] GetClientExcelViewCommandRequest request)
    {
        GetClientExcelViewCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
}