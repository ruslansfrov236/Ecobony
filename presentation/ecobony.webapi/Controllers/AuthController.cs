using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ecobony.webapi.Controllers
{
    public class AuthController(IMediator mediator) : Controller
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAsyncCommandRequest request)
        {
            LoginAsyncCommandResponse response= await mediator.Send(request);
            return View(response);
        }
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] CreateUserCommandRequest request)
        {
            CreateUserCommandResponse response = await mediator.Send(request);
            return View(response);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> Google([FromBody] GoogleLoginAsyncCommandRequest request)
        {
            GoogleLoginAsyncCommandResponse response = await mediator.Send(request);
            return View(response);
        }
        [HttpPost("facebook-login")]
        public async Task<IActionResult> Facebook([FromBody] FacebookLoginAsyncCommandRequest request)
        {
            FacebookLoginAsyncCommandResponse response = await mediator.Send(request);
            return View(response);
        }
    }
}
