using ecobony.application.Feauters.Query.UserHistory;
using ecobony.application.Feauters.Query.UserHistory.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query
{
    public class SearchQueryHandler(IUserHistoryService userHistoryService) : IRequestHandler<SearchQueryRequest, SearchQueryResponse>
    {
        public async Task<SearchQueryResponse> Handle(SearchQueryRequest request, CancellationToken cancellationToken)
        {
            var history = await userHistoryService.SearchAsync(request.keyword);


            return new SearchQueryResponse()
            {
                UserHistory = history
            };
        }
    }
}
