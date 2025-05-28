namespace ecobony.application.Feauters.Command;

public class AssignRoleDeleteUserCommandRequest : IRequest<AssignRoleDeleteUserCommandResponse>
{
    public string  userId { get; set; }
}