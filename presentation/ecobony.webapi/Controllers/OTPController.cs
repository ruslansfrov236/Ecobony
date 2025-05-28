using ecobony.application.Feauters.Command;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OTPController(IMediator mediator):ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] VerificationCodeAsyncCommandRequest request)
    {
        VerificationCodeAsyncCommandResponse response = await mediator.Send(request);
        return Ok(response);
    }

    [HttpPost("{email}")]
    public async Task<IActionResult> EmailConfirmed([FromRoute] EmailConfirmedCommandRequest request)
    {
        EmailConfirmedCommandResponse response=await mediator.Send(request);
        return Ok(response);
    }
    
}