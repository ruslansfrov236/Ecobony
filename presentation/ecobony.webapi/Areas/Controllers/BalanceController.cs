


namespace ecobony.webapi.Areas.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class BalanceController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetAdminBalanceAllQueryRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> Index([FromRoute] GetBalanceByIdQueryRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("restore-delete/{id}")]
        public async Task<IActionResult> RestoreDelete([FromRoute] RestoreBalanceCommandRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }



        [HttpPut("soft-delete/{id}")]
        public async Task<IActionResult> SoftDelete([FromRoute] SoftBalanceCommandRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
         [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete([FromRoute] DeleteBalanceCommandRequest request)
        {
            var result = await mediator.Send(request);
            return Ok(result);
        }
    }
}