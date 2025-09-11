
namespace ecobony.webapi.Filter;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> _logger ,RequestDelegate _next)
{
 

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var isoCode = Thread.CurrentThread.CurrentCulture.Name;

        var response = exception switch
        {
            NotFoundException => new
                { StatusCode = 404, Message = "Resource not found", Details = new { error = exception.Message ,  culture=isoCode } },
            BadRequestException => new
                { StatusCode = 400, Message = "Validation error", Details = new { error = exception.Message ,  culture = isoCode } },
            CustomUnauthorizedException => new
                { StatusCode = 401, Message = "Unauthorized access", Details = new { error = exception.Message ,  culture = isoCode } },
            _ => new
            {
                StatusCode = 500, Message = "Internal server error", Details = new { error = exception.Message, culture = isoCode }
            }
        };

        context.Response.StatusCode = response.StatusCode;
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}