using ecobony.application.Feauters.Query.UserHistory;
using ecobony.application.Feauters.Query.UserHistory.Search;
using ecobony.domain.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecobony.webapi.Areas.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
    public class UserHistoryController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] GetUserHistoryAdminQueryRequest request)
        {
            GetUserHistoryAdminQueryResponse response = await mediator.Send(request);

            return Ok(response);
        }
        [HttpGet("get-by-user/{userId}")]
        public async Task<IActionResult> Index ([FromRoute]GetByUserHistoryIdQueryRequest request)
        {
            GetByUserHistoryIdQueryResponse response = await mediator.Send(request);

            return Ok(response);
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery]SearchQueryRequest request)
        {
           SearchQueryResponse response = await mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-page-result")]
        public async Task<IActionResult> GetPagedResult([FromQuery] GetPagedQueryRequest request)
        {
            GetPagedQueryResponse response = await mediator.Send(request); return Ok(response);
        }
        [HttpGet("get-action-statistics")]
        
        public async Task<IActionResult> GetActionStatisticsAsync([FromQuery] GetActionStatisticsQueryRequest request)
        {
            GetActionStatisticsQueryResponse response = await mediator.Send(request);

            return Ok(response);
        }
        [HttpGet("get-action-type")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public async Task<IActionResult> GetActionTypeAsync([FromQuery]GetByActionTypeQueryRequest request)
        {
            GetByActionTypeQueryResponse response = await mediator.Send(request);

            return Ok(response);
        }

    }
}
