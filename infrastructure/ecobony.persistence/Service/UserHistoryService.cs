





using System.Linq.Expressions;

namespace ecobony.persistence.Service
{
    public class UserHistoryService(IUserHistoryReadRepository _read, IUserHistoryWriteRepository _write, UserManager<AppUser> _manager, IHttpContextAccessor _contextAccessor,
        HttpClient _httpClient, IMemoryCache _cache) : IUserHistoryService
    {
        public async Task<bool> Create()
        {
            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name ?? throw new NotFoundException("Not found username");
            AppUser? user = await _manager.FindByNameAsync(username) ?? throw new NotFoundException("Not found user");
            var method = _contextAccessor?.HttpContext?.Request?.Method ?? throw new NotFoundException("Not found method");
            var ip = _contextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString();


            if (ip == "::1" || ip == "127.0.0.1")
            {
                ip = await _httpClient.GetStringAsync("https://api.ipify.org");
            }
            var path = _contextAccessor.HttpContext.Request.Path.Value;
            if (_contextAccessor.HttpContext.Request.Method != "GET" ||
                !_contextAccessor.HttpContext.Request.Headers["Accept"].ToString().Contains("text/html") ||
                path.Contains(".js") || path.Contains(".css") || path.Contains(".png") ||
                path.Contains(".jpg") || path.Contains("favicon"))
            {
                var userAgent = _contextAccessor?.HttpContext?.
               Request?.Headers["User-Agent"].ToString();

                ActionType actionType = ActionType.All;
                switch (method.ToUpper())
                {
                    case "POST":
                        actionType = ActionType.Create;
                        break;
                    case "PUT":
                    case "PATCH":
                        actionType = ActionType.Update;
                        break;
                    case "DELETE":
                        actionType = ActionType.Delete;
                        break;
                    case "GET":
                        actionType = ActionType.All;
                        break;
                }


                UserHistory? result;


                if (!_cache.TryGetValue(ip, out result))
                {
                    try
                    {
                        var response = await _httpClient.GetStringAsync($"https://ipinfo.io/{ip}/json");
                        result = JsonSerializer.Deserialize<UserHistory>(response);

                        if (result != null)
                        {
                            _cache.Set(ip, result, TimeSpan.FromHours(1));
                        }
                    }
                    catch
                    {
                        result = new UserHistory(); 
                    }
                }
                var history = new UserHistory()
                {
                    IpAddress = ip,
                    UserId = user.Id,
                    UserAgent = userAgent,
                    UserName = user.UserName,
                    ActionType = actionType,
                    Loc = result.Loc,
                    City = result.City,
                    Country = result.Country,
                    Org = result.Org,
                    Region = result.Region,
                    Postal = result.Postal,
                    Timezone = result.Timezone


                };
                await _write.AddAsync(history);
                await _write.SaveChangegesAsync();
                return true;
            }

            return false;


        }

        public async Task<bool> Delete(string id)
        {
            var history = await _read.GetByIdAsync(id) ?? throw new NotFoundException("User history not found!");

             _write.Delete(history);
            _write.SaveChangegesAsync();
            return true;
        }

        public async Task<Dictionary<ActionType, int>> GetActionStatisticsAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _read
                .GetFilter(s => s.CreateAt >= startDate.ToLocalTime()
                                && s.CreateAt <= endDate.ToLocalTime()
                                && s.isDeleted == false)
                .OrderByDescending(s => s.UpdateAt != null ? s.UpdateAt : s.CreateAt)
        .ToListAsync();

            var result = data
                .GroupBy(s => s.ActionType)
                .ToDictionary(g => g.Key, g => g.Count());

            return result;
        }


        public async Task<List<UserHistory>> GetAdminAsync()
        => await _read.GetAll().ToListAsync();

        public async Task<List<UserHistory>> GetByActionTypeAsync(ActionType actionType)
    => await _read
        .GetFilter(a => a.ActionType == actionType && a.isDeleted == false)
        .OrderByDescending(s => s.UpdateAt != null ? s.UpdateAt : s.CreateAt)
        .ToListAsync();


        public async Task<List<UserHistory>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _read.GetFilter(s => s.CreateAt > startDate.ToLocalTime() && s.CreateAt < endDate.ToLocalTime() && s.isDeleted == false)
                .OrderByDescending(s => s.UpdateAt != null ? s.UpdateAt : s.CreateAt)
                .ToListAsync();
            return data;
        }

        public async Task<List<UserHistory>> GetByUserIdAsync(string userId)
        {
            AppUser? user = await _manager.FindByIdAsync(userId) ?? throw new NotFoundException("User is not found");

            var history = await _read.GetFilter(a=>a.isDeleted==false && a.UserId==user.Id).ToListAsync() ?? throw new NotFoundException($"{user.UserName} history  is not found");

            return history;
        }

        public async Task<UserHistory> GetClientAll()
        {
            var username = _contextAccessor?.HttpContext?.User?.Identity?.Name ?? throw new NotFoundException("Not found username");
            AppUser? user = await _manager.FindByNameAsync(username) ?? throw new NotFoundException("Not found user");
            var history = await _read.GetSingleAsync(a => a.UserId == user.Id && a.isDeleted == false) ?? throw new NotFoundException($"{user.UserName} history  is not found");
            return history;
        }

        public async Task<Dictionary<DateTime, int>> GetDailyActivityAsync(DateTime startDate, DateTime endDate)
        {
            var data = await _read
            .GetFilter(s => s.CreateAt >= startDate.ToLocalTime()
                            && s.CreateAt <= endDate.ToLocalTime()
                            && s.isDeleted == false)
            .OrderByDescending(s => s.UpdateAt != null ? s.UpdateAt : s.CreateAt)
    .ToListAsync();

            var result = data
                .GroupBy(s => s.CreateAt.Date)
                .ToDictionary(g => g.Key, g => g.Count());

            return result;
        }

      

        public async Task<int> GetLoginCountAsync(DateTime startDate, DateTime endDate)
        {
        
            
            var count = await _manager.Users
                .Where(t => t.RefreshTokenEndDate <= startDate && t.RefreshTokenEndDate >= endDate) // Token hələ aktivdirsə
                .Select(t => t.Id)
                .Distinct()
                .CountAsync();

            return count;
        }


        

        public async Task<int> GetOnlineUserCountAsync()
        => await  _manager.Users.Where(a => a.IsOnline == true).CountAsync();

      
public async Task<domain.Dto_s.PagedResult<UserHistory>> GetPagedAsync(int pageNumber, int pageSize, string sortBy, bool isDescending)
    {
        if (pageNumber < 1) pageNumber = 1;

            var query = _read.GetAll();

        
        var parameter = Expression.Parameter(typeof(UserHistory), "x");
        var property = Expression.Property(parameter, sortBy);
        var lambda = Expression.Lambda(property, parameter);

        var orderByMethod = isDescending ? "OrderByDescending" : "OrderBy";
        var orderByExpression = Expression.Call(
            typeof(Queryable),
            orderByMethod,
            new Type[] { typeof(UserHistory), property.Type },
            query.Expression,
            Expression.Quote(lambda)
        );

        query = query.Provider.CreateQuery<UserHistory>(orderByExpression);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return new domain.Dto_s.PagedResult<UserHistory>
        {
           

            TotalCount = totalCount,
            PageSize = pageSize,
            PageNumber = pageNumber,
            Items = items
        };
    }

        public async Task<List<UserHistory>> GetSuspiciousActivitiesAsync(DateTime startDate, DateTime endDate)
        {
            var activities = await _read.GetFilter(x => x.CreateAt >= startDate && x.CreateAt <= endDate)
      .ToListAsync();

          
            var suspiciousUsers = activities
                .GroupBy(x => x.UserId)
                .Where(g => g.Count() >= 10) 
                .SelectMany(g => g) 
                .ToList();

            return suspiciousUsers;
        }

        public async Task<int> GetTotalActionCountAsync()
        => await _manager.Users.CountAsync();

        public Task<bool> LogUserActionAsync(LogUserActionDto_s model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RestoreDelete(string id)
        {

            var history = await _read.GetSingleAsync(a => a.isDeleted == true && a.Id == Guid.Parse(id)) ?? throw new NotFoundException("User histoy is not found");

            history.isDeleted = false;
            _write.Update(history);
            await _write.SaveChangegesAsync();
            return true;
        }

        public async Task<List<UserHistory>> SearchAsync(string keyword)
         => await _read.GetFilter(a => a.UserName.ToLower().Contains(keyword.ToLower()) ||
         a.IpAddress.Contains(keyword.ToLower()) ||
         a.Org.Contains(keyword.ToLower()) || a.Loc.Contains(keyword.ToLower()) || a.UserAgent.Contains(keyword.ToLower()) || a.Country.Contains(keyword.ToLower())).ToListAsync();

    
        

        public async Task<bool> SoftDelete(string id)
        {
            var history = await _read.GetSingleAsync(a => a.isDeleted == false && a.Id == Guid.Parse(id)) ?? throw new NotFoundException("User histoy is not found");

            history.isDeleted = true;
            _write.Update(history);
         await   _write.SaveChangegesAsync();
            return true;
          
        }
    }
}