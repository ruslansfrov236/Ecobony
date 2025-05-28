using ecobony.application.Feauters.Command;
using ecobony.application.Feauters.Query;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Areas.Controllers;

[Area("Admin")]
[Route("api/[area]/[controller]")]
[ApiController]
public class CategoryController(IMediator mediator):ControllerBase
{
   [HttpGet]
    public async Task<IActionResult> Index([FromQuery]GetAdminCategoryAllCommandRequest request)
    {
        GetAdminCategoryAllCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Index([FromRoute] GetCategoryByIdCommandRequest request)
    {
        GetCategoryByIdCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateCategoryCommandRequest request)
    {
        CreateCategoryCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateCategoryCommandRequest request)
    {
        UpdateCategoryCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPut("soft-delete/{Id}")]
    public async Task<IActionResult> SoftDelete([FromRoute] SoftCategoryCommandRequest request)
    {
        SoftCategoryCommandResponse response = await mediator.Send(request);

        return Ok(response);
    }
    
    
    [HttpPut("restore/{Id}")]
    public async Task<IActionResult> SoftDelete([FromRoute] RestoreCategoryCommandRequest request)
    {
        RestoreCategoryCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }
    [HttpDelete("delete/{Id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteCategoryCommandRequest request)
    {
        DeleteCategoryCommandResponse response = await mediator.Send(request);

        return Ok(response);
    }
}
