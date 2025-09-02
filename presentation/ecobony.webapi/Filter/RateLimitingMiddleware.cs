using Microsoft.Extensions.Caching.Memory;

namespace ecobony.webapi.Filter
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;
        private const int LimitCount = 30;
        private static readonly TimeSpan LimitWindow = TimeSpan.FromSeconds(10);



        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var ip = GetClientIp(context);

          
            if (ip == "::1" || ip == "127.0.0.1")
            {
                if (!_cache.TryGetValue("ServerPublicIP", out string publicIp))
                {
                    var client = _httpClientFactory.CreateClient();
                    publicIp = (await client.GetStringAsync("https://api.ipify.org")).Trim();
                    _cache.Set("ServerPublicIP", publicIp, TimeSpan.FromHours(1));
                }
                ip = _cache.Get<string>("ServerPublicIP");
            }

            var key = $"RateLimit_{ip}";

            if (_cache.TryGetValue(key, out RateLimitInfo info))
            {
                if (info.Count >= LimitCount)
                {
                    await SendTooManyRequests(context);
                    return;
                }
                info.Count++;
                _cache.Set(key, info, info.ExpireAt - DateTime.UtcNow.ToLocalTime()); 
            }
            else
            {
                _cache.Set(key, new RateLimitInfo
                {
                    Count = 1,
                    ExpireAt = DateTime.UtcNow.ToLocalTime().Add(LimitWindow)
                }, LimitWindow);
            }

            await _next(context);
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

        private async Task SendTooManyRequests(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        }


        private class RateLimitInfo
        {
            public int Count { get; set; }
            public DateTime ExpireAt { get; set; }
        }
    }
}
