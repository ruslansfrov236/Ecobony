namespace ecobony.application.Feauters.Command;

public class UpdateRoleCommandRequest : IRequest<UpdateRoleCommandResponse>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public RoleModel RoleModel { get; set; }
}