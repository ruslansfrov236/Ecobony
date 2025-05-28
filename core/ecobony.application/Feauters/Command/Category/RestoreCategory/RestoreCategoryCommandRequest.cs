namespace ecobony.application.Feauters.Command;

public class RestoreCategoryCommandRequest:IRequest<RestoreCategoryCommandResponse>
{
    public string Id { get; set; }
}