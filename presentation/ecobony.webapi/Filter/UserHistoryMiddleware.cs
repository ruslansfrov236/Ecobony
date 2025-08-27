using ecobony.application.Services;

namespace ecobony.webapi.Filter
{
    public class UserHistoryMiddleware(RequestDelegate _next)
    {

      public async Task InvokeAsync(HttpContext context)
        {



            var userHistoryService  = context.RequestServices.GetRequiredService<IUserHistoryService>();
            await userHistoryService.Create();

            await _next(context);
        }
    }
}
