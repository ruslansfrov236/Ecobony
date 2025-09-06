using ecobony.application.Services;
using Microsoft.AspNetCore.Mvc.Filters;

public class UserTrackingActionFilter( IUserTrackingService _trackingService,
 IUserHistoryService _historyService) : IAsyncActionFilter
{
   

 

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var path = context.HttpContext.Request.Path.Value ;

    
        if (!path.Contains(".js") && !path.Contains(".css") &&
            !path.Contains(".png") && !path.Contains(".jpg") &&
            !path.Contains("favicon"))
        {
            await _trackingService.Create(path);
        }

        var endpoint = context.HttpContext.GetEndpoint();
        var requiresAuth = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAuthorizeData>() != null;

        if (requiresAuth)
        {
            await _historyService.Create();
        }

        await next();
    }
}
