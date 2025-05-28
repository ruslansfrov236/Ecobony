namespace ecobony.application.Feauters.Command;

public class SoftCategoryCommandRequest:IRequest<SoftCategoryCommandResponse>
{
    public string Id { get; set; }
}