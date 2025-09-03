using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetDailyActivityQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetDailyActivityQueryRequest, GetDailyActivityQueryResponse>
    {
        public async Task<GetDailyActivityQueryResponse> Handle(GetDailyActivityQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetDailyActivityAsync(request.StartDate, request.EndDate);

            return new GetDailyActivityQueryResponse()
            {
                Result = history,
            };
        }
    }
}
