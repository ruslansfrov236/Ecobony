using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetActionStatisticsQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetActionStatisticsQueryRequest, GetActionStatisticsQueryResponse>
    {
        public async Task<GetActionStatisticsQueryResponse> Handle(GetActionStatisticsQueryRequest request, CancellationToken cancellationToken)
        {
           var history = await userHistoryService.GetActionStatisticsAsync(request.StartDate , request.EndDate);

            return new GetActionStatisticsQueryResponse()
            {
                UserHistory = history
            };
        }
    }
}
