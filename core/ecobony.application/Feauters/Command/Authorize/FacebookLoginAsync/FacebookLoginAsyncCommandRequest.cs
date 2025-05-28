namespace ecobony.application.Feauters.Command;

public class FacebookLoginAsyncCommandRequest : IRequest<FacebookLoginAsyncCommandResponse>
{
    public string authToken  { get; set; } 
    public int accessTokenLifeTime  { get; set; }
}