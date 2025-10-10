using ecobony.application.Repository;
using ecobony.domain.Entities;
using ecobony.signair.Hubs;


namespace ecobony.signair.Services
{
    public class LogOptionsService : Hub
    {
        readonly private ILogOptionWriteRepository _logOptionWriteRepository;
        readonly private IHubContext<LogHub> _hubContext;

        public LogOptionsService(ILogOptionWriteRepository logOptionWriteRepository , IHubContext<LogHub> hubContext)
        {
            _hubContext = hubContext;
            _logOptionWriteRepository = logOptionWriteRepository;   
        }
        public async Task SendLog(string message, string level, string userName, string requestPath, string exception, DateTime timeStamp)
        {

            var logOptions = new LogOptions()
            {
                Message = message,
                Level = level,
                UserName = userName,
                RequestPath = requestPath,
                Exception = exception,
                TimeStamp = timeStamp
            };

            await _logOptionWriteRepository.AddAsync(logOptions);
            await _logOptionWriteRepository.SaveChangegesAsync();

            await  _hubContext.Clients.All.SendAsync(ReceiveHubName.ReceiveLog, logOptions);


        }
    }
}
