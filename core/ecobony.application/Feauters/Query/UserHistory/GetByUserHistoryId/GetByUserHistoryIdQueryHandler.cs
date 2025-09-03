using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetByUserHistoryIdQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetByUserHistoryIdQueryRequest, GetByUserHistoryIdQueryResponse>
    {
        public async Task<GetByUserHistoryIdQueryResponse> Handle(GetByUserHistoryIdQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetByUserIdAsync(request.Id);

            return new GetByUserHistoryIdQueryResponse()
            {
                UserHistory = history,
            };
        }
    }
}
