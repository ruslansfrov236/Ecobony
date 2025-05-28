namespace ecobony.application.Feauters.Query;

public class GetRoleByIdCommandHandler(IRoleService _roleService):IRequestHandler<GetRoleByIdCommandRequest, GetRoleByIdCommandResponse>
{
    public async Task<GetRoleByIdCommandResponse> Handle(GetRoleByIdCommandRequest request, CancellationToken cancellationToken)
    {
      var role =  await _roleService.GetById(request.Id);
        return new GetRoleByIdCommandResponse()
        {
            Role = role
        };
    }
}