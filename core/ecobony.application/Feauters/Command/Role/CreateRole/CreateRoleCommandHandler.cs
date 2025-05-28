namespace ecobony.application.Feauters.Command;

public class CreateRoleCommandHandler(IRoleService _roleService):IRequestHandler<CreateRoleCommandRequest, CreateRoleCommandResponse>
{
    public async Task<CreateRoleCommandResponse> Handle(CreateRoleCommandRequest request, CancellationToken cancellationToken)
    {
        await _roleService.Create(new CreateRolesDto_s()
        {
            Name = request.Name,
            RoleModel = request.RoleModel
        });
        return new CreateRoleCommandResponse();
    }
}