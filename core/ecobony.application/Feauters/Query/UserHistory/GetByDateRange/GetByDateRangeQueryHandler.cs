using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetByDateRangeQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetByDateRangeQueryRequest, GetByDateRangeQueryResponse>
    {
        public async Task<GetByDateRangeQueryResponse> Handle(GetByDateRangeQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetByDateRangeAsync(request.StartDate, request.EndDate);

            return new GetByDateRangeQueryResponse()
            {
                UserHistory = history
            };
        }
    }
}
