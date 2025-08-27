using System;
using System.Text.Json.Serialization;
namespace ecobony.domain.Entities
{
    public class UserHistory : BaseEntity
    {
        public string UserId { get; set; }
        public AppUser? User { get; set; }

        public string? UserName { get; set; }
        public ActionType ActionType { get; set; }

        public string? Description { get; set; }
        [JsonPropertyName("ip")]
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
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
    }
}
