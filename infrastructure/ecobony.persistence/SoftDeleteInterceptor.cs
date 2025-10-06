using ecobony.domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ecobony.persistence
{
    public class SoftDeleteInterceptor(
        IHttpContextAccessor httpContextAccessor
    ) : SaveChangesInterceptor
    {
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var httpContext = httpContextAccessor.HttpContext;
            string? username = httpContext?.User?.Identity?.Name;
            string? userId = httpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            UserManager<AppUser>? userManager = null;
            AppUser? user = null;

            if (!string.IsNullOrWhiteSpace(username))
            {
                userManager = httpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
                user = await userManager.FindByNameAsync(username) ?? throw new NotFoundException();
            }

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        // TrashCan log əlavə et
                        if (user != null)
                        {
                            var trash = new TrashCan
                            {
                                EntityName = entry.Entity.GetType().Name,
                                EntityId = entry.Entity.Id,
                                UserId = userId,
                                OperationAt=DateTime.UtcNow.ToLocalTime(),
                                OperationType=OperationType.Create,
                                UserName = username,
                                
                            };
                            await context.Set<TrashCan>().AddAsync(trash, cancellationToken);
                        }
                        break;

                    case EntityState.Modified:

                        // TrashCan log əlavə et
                        if (user != null)
                        {
                            var trash = new TrashCan
                            {
                                EntityName = entry.Entity.GetType().Name,
                                EntityId = entry.Entity.Id,
                                UserId = userId,
                                OperationAt = DateTime.UtcNow.ToLocalTime(),
                                OperationType = OperationType.Update,
                                UserName = username,
                              
                            };
                            await context.Set<TrashCan>().AddAsync(trash, cancellationToken);
                        }
                        break;

                    case EntityState.Deleted:
                        // Soft delete
                        entry.State = EntityState.Modified;
                        entry.Entity.isDeleted = true;

                        // TrashCan log əlavə et
                        if (user != null)
                        {
                            var trash = new TrashCan
                            {
                                EntityName = entry.Entity.GetType().Name,
                                EntityId = entry.Entity.Id,
                                UserId = userId,
                                OperationAt = DateTime.UtcNow.ToLocalTime(),
                                OperationType = OperationType.Delete,
                                UserName = username,
                                DeletedAt = DateTime.UtcNow
                            };
                            await context.Set<TrashCan>().AddAsync(trash, cancellationToken);
                        }
                        break;
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
