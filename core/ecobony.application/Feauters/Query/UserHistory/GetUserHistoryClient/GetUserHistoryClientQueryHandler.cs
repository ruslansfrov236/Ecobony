using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetUserHistoryClientQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetUserHistoryClientQueryRequest, GetUserHistoryClientQueryResponse>
    {
        public async Task<GetUserHistoryClientQueryResponse> Handle(GetUserHistoryClientQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetClientAll();

            return new GetUserHistoryClientQueryResponse()
            {
                UserHistory = history,
            };
        }
    }
}
