using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.domain.Dto_s
{
    public class CreateUserHistoryDto_s
    {
        public Guid UserId { get; set; }
        public AppUser? User { get; set; }

        public string? UserName { get; set; }
        public ActionType ActionType { get; set; }

        public string? Description { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }


        public string? City { get; set; }
        public string? Region { get; set; }
        public string? Country { get; set; }
        public string? Loc { get; set; }
        public string? Org { get; set; }
        public string? Postal { get; set; }
        public string? Timezone { get; set; }
    }
}
