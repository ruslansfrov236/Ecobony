namespace ecobony.application.Feauters.Command;

public class CreateRoleCommandRequest:IRequest<CreateRoleCommandResponse>
{
    public string Name { get; set; }
    public RoleModel RoleModel { get; set; }
}