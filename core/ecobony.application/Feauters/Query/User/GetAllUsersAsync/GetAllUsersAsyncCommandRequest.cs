namespace ecobony.application.Feauters.Query;

public class GetAllUsersAsyncCommandRequest : IRequest<GetAllUsersAsyncCommandResponse>
{
    public int page { get; set; }
    public int size { get; set; }
}