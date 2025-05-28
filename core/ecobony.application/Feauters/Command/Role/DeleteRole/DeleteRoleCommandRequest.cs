namespace ecobony.application.Feauters.Command;

public class DeleteRoleCommandRequest:IRequest<DeleteRoleCommandResponse>
{
    public string Id { get; set; }
}