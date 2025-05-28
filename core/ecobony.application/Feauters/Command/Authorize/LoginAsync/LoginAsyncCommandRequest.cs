namespace ecobony.application.Feauters.Command;

public class LoginAsyncCommandRequest : IRequest<LoginAsyncCommandResponse>
{
  public  string password { get; set; } 
 public string usernameOrEmail  { get; set; } 
 public int accessTokenLifeTime  { get; set; }
 public bool isSave { get; set; } = false;
}