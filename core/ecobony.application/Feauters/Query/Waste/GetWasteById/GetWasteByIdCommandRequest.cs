namespace ecobony.application.Feauters.Query;

public class GetWasteByIdCommandRequest : IRequest<GetWasteByIdCommandResponse>
{
    public string Id { get; set; }
}