<<<<<<< HEAD
﻿using ecobony.signair.Services;

public class SoftDeleteInterceptor(
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider _serviceProvider
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
        if (httpContext == null ||
            httpContext.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            return await base.SavingChangesAsync(eventData, result, cancellationToken);

        string? username = httpContext?.User?.Identity?.Name;
        AppUser? user = null;
        if (!string.IsNullOrWhiteSpace(username))
        {
            var userManager = httpContext?.RequestServices.GetService<UserManager<AppUser>>();
            if (userManager != null)
                user = await userManager.FindByNameAsync(username);
        }

        // TrashCan və bəzi entity-ləri atla
        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.Entity is TrashCan
                || entry.Entity is LogOptions
                || entry.Entity is UserHistory
                || entry.Entity is UserTracking)
                continue;

            var dto = new CreateTrashCanDto_s
            {
                EntityName = entry.Entity.GetType().Name,
                EntityId = entry.Entity.Id,
                UserId = user?.Id,
                OperationAt = DateTime.UtcNow.ToLocalTime(),
                UserName = user?.UserName ?? "Anonymous"
            };

            switch (entry.State)
            {
                case EntityState.Added:
                    dto.OperationType = OperationType.Create;
                    break;
                case EntityState.Modified:
                    dto.OperationType = OperationType.Update;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.isDeleted = true;
                    dto.OperationType = OperationType.Delete;
                    dto.DeletedAt = DateTime.UtcNow.ToLocalTime();
                    break;
            }

            // Yeni scope yaradılır ki, interceptor özünü təkrar çağırmasın
            using (var scope = _serviceProvider.CreateScope())
            {
                var trashCanService = scope.ServiceProvider.GetRequiredService<TrashCanSignairService>();

                if (dto.OperationType == OperationType.Create)
                    await trashCanService.SendTaskCreate(dto);
                else if (dto.OperationType == OperationType.Delete)
                    await trashCanService.SendTaskDeleteCreate(dto);
                else if (dto.OperationType == OperationType.Update)
                    await trashCanService.SendTaskCreate(dto);
            }
        }

       
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
=======
﻿using ecobony.domain.Entities;
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
>>>>>>> f72c4dc9a5651ca5e78be3a0b603e62c05e007c6
