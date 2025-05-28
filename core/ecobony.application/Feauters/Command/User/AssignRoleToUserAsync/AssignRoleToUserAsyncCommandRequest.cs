namespace ecobony.application.Feauters.Command;

public class AssignRoleToUserAsyncCommandRequest : IRequest<AssignRoleToUserAsyncCommandResponse>
{
    public string  userId { get; set; }
    public string[] Roles { get; set; }
}