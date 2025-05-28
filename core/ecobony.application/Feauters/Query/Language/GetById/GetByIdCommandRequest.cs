namespace ecobony.application.Feauters.Query.Language;

public class GetByIdCommandRequest:IRequest<GetByIdCommandResponse>
{
    public string Id { get; set; }
}