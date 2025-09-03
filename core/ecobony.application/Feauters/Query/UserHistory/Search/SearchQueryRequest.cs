using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory.Search
{
    public class SearchQueryRequest:IRequest<SearchQueryResponse>
    {
        public string? keyword { get; set; }
    }
}
