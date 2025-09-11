using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Services
{
    public interface ITrashCanService
    {
     Task<List<TrashCan>> GetClientAll();
        Task<List<TrashCan>> GetAdminAll();
        Task<PagedResult<string>> GetPagedResult(int pageNumber, int pageSize, string sortBy, bool isDescending);
    }
}
