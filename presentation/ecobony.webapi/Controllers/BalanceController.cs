using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager, User")]
    public class BalanceController(IMediator mediator) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetClientBalanceAllCommandRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBalanceCommandRequest request)
        {
            var result = await mediator.Send(request);
            return StatusCode((int)HttpStatusCode.Created, result);
        }
      
    }
}