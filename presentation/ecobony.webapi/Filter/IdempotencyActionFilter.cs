using ecobony.domain.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Threading.Tasks;

namespace ecobony.webapi.Filter
{
    public class IdempotencyActionFilter( IHttpContextAccessor _httpContextAccessor,
    IBalanceTransferReadRepository _balanceTransferRead,
     IBonusComunityReadRepository _bonusComunityReadRepository,
     UserManager<AppUser> userManager,
        IBalanceReadRepository balanceReadRepository,
            IBonusReadRepository bonusReadRepository
     ) : IAsyncActionFilter
    {
        

     

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var endpoint = context.HttpContext.GetEndpoint();
            var requiresAuth = endpoint?.Metadata?.GetMetadata<Microsoft.AspNetCore.Authorization.IAuthorizeData>() != null;

            if (!requiresAuth)
            {
                await next();
                return;
            }
            //  adı yoxlanır
            var username = _httpContextAccessor.HttpContext.User.Identity.Name
                ?? throw new NotFoundException("User is name not found");
            AppUser? user = await userManager.FindByNameAsync(username) ??  
                throw new NotFoundException("User is not found"); ;
            // Controller adı yoxlanır
            var controllerRole = httpContext?.GetRouteData()?.Values["controller"]?.ToString()
                ?? throw new NotFoundException("Controller is not found");

            // HTTP metodu yoxlanır
            var method = httpContext.Request.Method
                ?? throw new NotFoundException("Method is not found");

            // Idempotency-Key başlığı alınır
            var idempotencyKey = httpContext.Request.Headers["Idempotency-Key"].FirstOrDefault();

            if (string.IsNullOrEmpty(idempotencyKey))
            {
                context.Result = new BadRequestObjectResult("Idempotency-Key header is required.");
                return;
            }

            // Yalnız POST sorğular üçün yoxlama aparılır
            if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                switch (controllerRole.ToLower())
                {
                    case "bonus":
                        var bonusUser = await bonusReadRepository.GetSingleAsync(a=>a.UserId==user.Id) 
                               ?? throw new NotFoundException("Bonus is user not found");
                        var bonus = await _bonusComunityReadRepository
                            .GetSingleAsync(a => a.IdempotencyKey == idempotencyKey && a.BonusId==bonusUser.Id);

                        if (bonus != null)
                        {
                          
                            context.Result = new ConflictObjectResult("Duplicate request detected.");
                            return;
                        }
                        else
                        {
                            context.Result = new ObjectResult(new { message = "Created successfully" })
                            {
                                StatusCode = StatusCodes.Status201Created
                            };
                            return;
                           
                        }
                            break;

                    case "balance":
                        var balance = await balanceReadRepository.GetSingleAsync(a=>a.UserId== user.Id) ??
                            throw new NotFoundException("Balance user is not found");
                        var transfer = await _balanceTransferRead
                            .GetSingleAsync(a => a.IdempotencyKey == idempotencyKey && a.BalanceId==balance.Id);

                        if (transfer != null)
                        {
                            context.Result = new ConflictObjectResult("Duplicate request detected.");
                            return;
                        }
                        else
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Created;
                            return;
                        }
                        break;

                   
                }
            }

           
            await next();
        }
    }
}
