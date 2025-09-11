using ecobony.domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Net.Http;

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
            var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();

            var context = eventData.Context;
            if (context == null)
                return await base.SavingChangesAsync(eventData, result, cancellationToken);

            // Silinmiş entity-ləri tap
            var deletedEntities = context.ChangeTracker
                                         .Entries<BaseEntity>()
                                         .Where(e => e.State == EntityState.Deleted);

            // İstifadəçi məlumatını götür
            var username = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
                      

            if (!string.IsNullOrWhiteSpace(username))
            {
                AppUser user = await userManager.FindByNameAsync(username)
                                 ?? throw new NotFoundException();

                foreach (var entry in deletedEntities)
                {
                    // Hard delete əvəzinə soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.isDeleted = true;

                    // TrashCan-a log əlavə et
                    var trash = new TrashCan
                    {
                        EntityName = entry.Entity.GetType().Name,

                        EntityId = entry.Entity.Id,
                        UserName = username,
                        DeletedAt = DateTime.UtcNow
                    };



                    await context.Set<TrashCan>().AddAsync(trash);

                }

            }

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}