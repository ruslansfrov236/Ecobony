namespace ecobony.application.Feauters.Query;

public class GetWasteCategoryCommandRequest : IRequest<GetWasteCategoryCommandResponse>
{
    public string categoryId { get; set; }
}