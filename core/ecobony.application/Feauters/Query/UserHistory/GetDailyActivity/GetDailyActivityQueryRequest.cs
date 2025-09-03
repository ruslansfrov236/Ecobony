using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetDailyActivityQueryRequest:IRequest<GetDailyActivityQueryResponse>
    {
        public DateTime StartDate { get; set; } 
        public DateTime EndDate { get; set; }
    }
}
