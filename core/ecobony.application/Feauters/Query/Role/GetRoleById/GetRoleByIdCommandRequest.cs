namespace ecobony.application.Feauters.Query;

public class GetRoleByIdCommandRequest : IRequest<GetRoleByIdCommandResponse>
{
    public string Id { get; set; }
}