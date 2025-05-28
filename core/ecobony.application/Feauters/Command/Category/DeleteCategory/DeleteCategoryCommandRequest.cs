namespace ecobony.application.Feauters.Command;

public class DeleteCategoryCommandRequest:IRequest<DeleteCategoryCommandResponse>
{
    public string Id { get; set; }
}