using ecobony.domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace ecobony.persistence
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SoftDeleteInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var httpContext = _httpContextAccessor.HttpContext;
            string? userId = null;
            string? userName = null;

            if (httpContext != null && httpContext.User?.Identity?.IsAuthenticated == true)
            {
                userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                userName = httpContext.User.Identity?.Name;
            }

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreateAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdateAt = DateTime.UtcNow;
                        // Yalnız authenticated user varsa TrashCan əlavə et
                        if (userId != null)
                        {
                            var trash = new TrashCan
                            {
                                EntityName = entry.Entity.GetType().Name,
                                EntityId = entry.Entity.Id,
                                UserId = userId,
                                UserName = userName,
                                DeletedAt = DateTime.UtcNow
                            };

                            context.Set<TrashCan>().Add(trash);
                        }
                        break;

                    case EntityState.Deleted:
                        // Soft delete
                        entry.State = EntityState.Modified;
                        entry.Entity.isDeleted = true;

                        // Yalnız authenticated user varsa TrashCan əlavə et
                        if (userId != null)
                        {
                            var trash = new TrashCan
                            {
                                EntityName = entry.Entity.GetType().Name,
                                EntityId = entry.Entity.Id,
                                UserId = userId,
                                UserName = userName,
                                DeletedAt = DateTime.UtcNow
                            };

                            context.Set<TrashCan>().Add(trash);
                        } 
                        break;
                }
            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
