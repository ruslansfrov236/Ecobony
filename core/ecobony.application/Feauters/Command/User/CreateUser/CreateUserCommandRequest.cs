namespace ecobony.application.Feauters.Command;

public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
{
    public string? UserName { get; set; }
    public string? Email {get; set;}
    public string? Password{get; set;}
}