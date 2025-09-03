using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetPagedQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<GetPagedQueryRequest, GetPagedQueryResponse>
    {
        public async Task<GetPagedQueryResponse> Handle(GetPagedQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.GetPagedAsync(request.pageNumber, request.pageSize, request.sortBy, request.isDescending);

            return new GetPagedQueryResponse()
            {
                PagedResult = history
            };
        }
    }
}
