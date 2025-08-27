using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.domain.Dto_s
{
    public class LogUserActionDto_s
    {
       
            public string UserId { get; set; } = default!;
            public string? Username { get; set; }
            public ActionType ActionType { get; set; }
            public string? Resource { get; set; }
            public string? Description { get; set; }
            public string? IpAddress { get; set; }
            public string? UserAgent { get; set; }
        

    }
}
