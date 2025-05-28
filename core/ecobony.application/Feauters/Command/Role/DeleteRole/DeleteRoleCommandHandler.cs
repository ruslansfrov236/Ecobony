namespace ecobony.application.Feauters.Command;

public class DeleteRoleCommandHandler(IRoleService _roleService):IRequestHandler<DeleteRoleCommandRequest, DeleteRoleCommandResponse>
{
    public async Task<DeleteRoleCommandResponse> Handle(DeleteRoleCommandRequest request, CancellationToken cancellationToken)
    {
        await _roleService.Delete(request.Id);
        return new DeleteRoleCommandResponse();
    }
}