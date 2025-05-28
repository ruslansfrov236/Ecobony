
namespace ecobony.domain.Dto_s;

public class FacebookAccessToken_Dto_s
{
    [JsonPropertyName("access_token")]
    public string AccessToken {get; set;}
    [JsonPropertyName("token_type")]
    public string TokenType {get; set;}
    
}

