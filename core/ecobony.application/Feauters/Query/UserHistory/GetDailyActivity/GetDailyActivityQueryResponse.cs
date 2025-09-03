using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.application.Feauters.Query.UserHistory
{
    public class GetDailyActivityQueryResponse
    {
        public Dictionary<DateTime, int> Result { get; set; }
    }
}
