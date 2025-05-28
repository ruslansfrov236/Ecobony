namespace ecobony.application.Feauters.Command;

public class UpdatePasswordCommandRequest : IRequest<UpdatePasswordCommandResponse>
{
  public   string userId { get; set; }
  public string resetToken { get; set; } 
  public string newPassword { get; set; }
    
}