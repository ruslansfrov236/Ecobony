using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetUserHistoryAdminQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetUserHistoryAdminQueryRequest, GetUserHistoryAdminQueryResponse>
    {
        public async Task<GetUserHistoryAdminQueryResponse> Handle(GetUserHistoryAdminQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetAdminAsync();
            return new GetUserHistoryAdminQueryResponse()
            {
                UserHistory = history,  
            };
        }
    }
}
