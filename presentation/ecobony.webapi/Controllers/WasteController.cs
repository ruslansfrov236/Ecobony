using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class WasteController(IMediator mediator):ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery]GetClientWasteAllCommandRequest request)
    {
        GetClientWasteAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("trash")]
    public async Task<IActionResult> Index([FromQuery] GetWasteTrashCommandRequest request)
    {
        GetWasteTrashCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> Index([FromRoute] GetWasteCategoryCommandRequest request)
    {
        GetWasteCategoryCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWasteCommandRequest request)
    {
        CreateWasteCommandResponse response = await mediator.Send(request);
        return StatusCode((int)HttpStatusCode.Created);
      
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWasteCommandRequest request)
    {
        UpdateWasteCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("restore/{Id}")]
    public async Task<IActionResult> Restore([FromBody] RestoreWasteCommandRequest request)
    {
        RestoreWasteCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpPut("soft-delete/{Id}")]
    public async Task<IActionResult> SoftDelete([FromBody] SoftWasteCommandRequest request)
    {
        SoftWasteCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteWasteCommandRequest request)
    {
        DeleteWasteCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    
}
