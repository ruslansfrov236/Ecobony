using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetByActionTypeQueryRequest:IRequest<GetByActionTypeQueryResponse>
    {
        public ActionType ActionType { get; set; }
    }
}
