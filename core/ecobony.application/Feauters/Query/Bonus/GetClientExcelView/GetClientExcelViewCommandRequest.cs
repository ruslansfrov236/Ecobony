namespace ecobony.application.Feauters.Query;

public class GetClientExcelViewCommandRequest : IRequest<GetClientExcelViewCommandResponse>
{
   public DateTime StartDate { get; set; } 
   public DateTime  EndDate { get; set; }
}