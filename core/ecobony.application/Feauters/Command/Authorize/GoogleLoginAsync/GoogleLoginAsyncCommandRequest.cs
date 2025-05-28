namespace ecobony.application.Feauters.Command;

public class GoogleLoginAsyncCommandRequest : IRequest<GoogleLoginAsyncCommandResponse>
{
    public string IdToken  { get; set; } 
    public int accessTokenLifeTime  { get; set; }
}