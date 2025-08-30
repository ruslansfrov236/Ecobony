using ecobony.application.Repository;
using ecobony.application.Services;
using ecobony.domain.Entities;
using ecobony.signair.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ecobony.signair.Services
{
    public class UserTrackingService(IUserTrackingWriteRepository _write, IHttpContextAccessor _contextAccessor, UserManager<AppUser> _userManager, HttpClient _httpClient , IHubContext<UserTrackingHub> _hubContext) : IUserTrackingService
    {
        public async Task<bool> Create(string path)
        {
            var context = _contextAccessor.HttpContext;
            string username = _contextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";
            


            var user = await _userManager.FindByNameAsync(username);
            var ip = GetClientIp(context);
            if (ip == "::1" || ip == "127.0.0.1")
            {
                ip = await _httpClient.GetStringAsync("https://api.ipify.org");

            }

            var userAgent = context.Request.Headers["User-Agent"].ToString();
            var query = context.Request.QueryString.ToString();
            var referer = context.Request.Headers["Referer"].ToString();
            var response = await _httpClient.GetStringAsync($"https://ipinfo.io/{ip}/json");
            var result = JsonSerializer.Deserialize<UserTracking>(response);

            if (user is not null)
            {

            }
          

                var userTracking = new UserTracking
                {
                    Ip = ip,
                    Path = path,
                    UserAgent = userAgent,
                    UserId = user?.Id,
                    UserName = username,
                    Query = query,
                    Referer = referer,
                    Country = result?.Country,
                    Region = result?.Region,
                    City = result?.City,
                    Postal = result?.Postal,
                    Loc = result?.Loc,
                    Org = result?.Org,
                    Timezone = result?.Timezone,
                    Hostname = result?.Hostname
                };

                await _write.AddAsync(userTracking);
                await _write.SaveChangegesAsync();
                await _hubContext.Clients.All.SendAsync(ReceiveHubName.UserTrackingAll, true);

            return true;

            }
        private string GetClientIp(HttpContext context)
        {
            // Proxy arxasında olarsa X-Forwarded-For istifadə et
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0].Trim();
            }
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
    }

