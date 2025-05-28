namespace ecobony.application.Feauters.Query.Header;

public class GetByIdCommandRequest:IRequest<GetByIdCommandResponse>
{
    public string Id { get; set; }
}