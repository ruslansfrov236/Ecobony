

using ecobony.application.Services;
using ecobony.signair.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ecobony.webapi.Filter
{
    public class UserTrackingMiddleware(RequestDelegate _next)
    {
       
            public async Task InvokeAsync(HttpContext context)
            {

                var path = context.Request.Path.Value;
                if (context.Request.Method != "GET" ||
                    !context.Request.Headers["Accept"].ToString().Contains("text/html") ||
                    path.Contains(".js") || path.Contains(".css") || path.Contains(".png") ||
                    path.Contains(".jpg") || path.Contains("favicon"))
                {
                    await _next(context);
                    return;
                }

                var userTrackingService = context.RequestServices.GetRequiredService<IUserTrackingService>();
                await userTrackingService.Create(path);

                await _next(context);
            }

        }
    }

