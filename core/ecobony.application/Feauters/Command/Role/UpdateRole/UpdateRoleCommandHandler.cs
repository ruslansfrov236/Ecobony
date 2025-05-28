namespace ecobony.application.Feauters.Command;

public class UpdateRoleCommandHandler(IRoleService _roleService):IRequestHandler<UpdateRoleCommandRequest, UpdateRoleCommandResponse>
{
    public async Task<UpdateRoleCommandResponse> Handle(UpdateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        await _roleService.Update(new UpdateRolesDto_s()
        {
            Id = request.Id,
            Name = request.Name,
            RoleModel = request.RoleModel
        });
        return new UpdateRoleCommandResponse();
    }
}