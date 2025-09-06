using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobony.domain.Entities
{
    public class UserTracking:BaseEntity
    {
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }

        public string? UserName { get; set; }
        public AppUser? User { get; set; }

        public string? UserId { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("loc")]
        public string? Loc { get; set; }

        [JsonPropertyName("org")]
        public string? Org { get; set; }

        [JsonPropertyName("postal")]
        public string? Postal { get; set; }

        [JsonPropertyName("timezone")]
        public string? Timezone { get; set; }

        [JsonPropertyName("hostname")]
        public string? Hostname { get; set; }

        public string? Path { get; set; }
        public string? Query { get; set; }
        public string? UserAgent { get; set; }
        public string? Referer { get; set; }

    }
}
