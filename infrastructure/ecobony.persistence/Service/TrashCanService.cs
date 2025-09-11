using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using e = ecobony.domain.Entities;
namespace ecobony.persistence.Service
{
    public class TrashCanService(
        ITrashCanReadRepository trashCanReadRepository,
        IHttpContextAccessor httpContextAccessor,AppDbContext context, 
        UserManager<AppUser> userManager) : ITrashCanService
    {
        public async Task<List<TrashCan>> GetAdminAll()
        {
            var username = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? throw new NotFoundException("User name is not found ");
            AppUser user = await userManager.FindByNameAsync(username);

            var trash = await trashCanReadRepository.GetFilter(a => a.UserId == user.Id && a.UserName == username && a.isDeleted == true).ToListAsync();

            return trash;
        }


        public async Task<List<TrashCan>> GetClientAll()
        => await trashCanReadRepository.GetFilter(a => a.isDeleted == false).ToListAsync();

        public async Task<domain.Dto_s.PagedResult<string>> GetPagedResult(int pageNumber, int pageSize, string sortBy, bool isDescending)
        {
            var username = httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? throw new CustomUnauthorizedException("User name is not found ");
            AppUser? user = await userManager.FindByNameAsync(username) ?? throw new NotFoundException();
            var trash = await trashCanReadRepository.GetSingleAsync(a => a.UserId == user.Id && a.UserName == username && a.EntityName == sortBy);

            IQueryable<string>? query = sortBy switch
            {
                "Category" => context.Set<GetCategoryDto_s>().Where(a => a.isDeleted).Select(a => a.Name),
                "Header" => context.Set<GetHeadetDto>().Where(a => a.isDeleted).Select(a => a.Title),
                "Language" => context.Set<Language>().Where(a => a.isDeleted).Select(a => a.Name),
                "Location" => context.Set<ecobony.domain.Entities.Location>().Where(a => a.isDeleted).Select(a => a.Country),
                "Waste" => context.Set<Waste>().Where(a => a.isDeleted).Select(a => a.Title),
                "WasteProcess" => context.Set<WasteProcess>().Where(a => a.isDeleted).Select(a => a.Weight.ToString()),
              
                _ => throw new ArgumentException($"Entity '{sortBy}' tapılmadı")
            };



            query = isDescending
               ? query.OrderByDescending(x => x)
               : query.OrderBy(x => x);

            // Paging
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new domain.Dto_s.PagedResult<string>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

        }
    }
}
