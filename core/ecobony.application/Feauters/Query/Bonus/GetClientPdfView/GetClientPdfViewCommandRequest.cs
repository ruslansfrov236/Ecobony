namespace ecobony.application.Feauters.Query;

public class GetClientPdfViewCommandRequest : IRequest<GetClientPdfViewCommandResponse>
{  public DateTime StartDate { get; set; } 
    public DateTime  EndDate { get; set; }
}