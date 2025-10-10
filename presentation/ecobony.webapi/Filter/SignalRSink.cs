using ecobony.signair.Services;
using Serilog.Core;
using Serilog.Events;
using System;

namespace ecobony.webapi.Filter
{
    public class SignalRSink : ILogEventSink
    {
        private readonly LogOptionsService _logHubService;

        public SignalRSink(LogOptionsService logHubService)
        {
            _logHubService = logHubService;
        }

        public void Emit(LogEvent logEvent)
        {
            Task.Run(async () =>
            {
                try
                {
                    var message = logEvent.RenderMessage();
                    var level = logEvent.Level.ToString();
                    var timeStamp = logEvent.Timestamp.UtcDateTime;
                    var exception = logEvent.Exception?.ToString() ?? string.Empty;
                    var userName = logEvent.Properties.ContainsKey("UserName")
                        ? logEvent.Properties["UserName"].ToString().Trim('"')
                        : "Anonymous";
                    var requestPath = logEvent.Properties.ContainsKey("RequestPath")
                        ? logEvent.Properties["RequestPath"].ToString().Trim('"')
                        : "";

                    await _logHubService.SendLog(message, level, userName, requestPath, exception, timeStamp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("SignalRSink Emit error: " + ex);
                }
            });

        }
    }
}
