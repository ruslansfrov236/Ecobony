namespace ecobony.domain.Dto_s;

public class FacebookUserInfoResponse
{
    [JsonPropertyName("id")]
    public string? Id {get; set;}
    [JsonPropertyName("email")]
    public string? Email {get; set;}
    [JsonPropertyName("name")]
    public string? Name {get; set;}
}