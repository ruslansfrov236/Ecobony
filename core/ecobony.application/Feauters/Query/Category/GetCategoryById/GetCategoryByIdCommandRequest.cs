namespace ecobony.application.Feauters.Query;

public class GetCategoryByIdCommandRequest:IRequest<GetCategoryByIdCommandResponse>
{
    public string Id { get; set; }
}