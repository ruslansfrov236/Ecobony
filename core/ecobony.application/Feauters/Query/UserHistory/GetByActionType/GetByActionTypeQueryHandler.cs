using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetByActionTypeQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetByActionTypeQueryRequest, GetByActionTypeQueryResponse>
    {
        public async Task<GetByActionTypeQueryResponse> Handle(GetByActionTypeQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetByActionTypeAsync(request.ActionType);

            return new GetByActionTypeQueryResponse()
            {
                UserHistory = history,
            };
        }
    }
}
