namespace ecobony.application.Feauters.Query;

public class GetRoleAllCommandHandler(IRoleService _roleService):IRequestHandler<GetRoleAllCommandRequest, GetRoleAllCommandResponse>
{
    public async Task<GetRoleAllCommandResponse> Handle(GetRoleAllCommandRequest request, CancellationToken cancellationToken)
    {
      var role=  await _roleService.GetAll();
        return new GetRoleAllCommandResponse()
        {
            Role = role
        };
    }
}