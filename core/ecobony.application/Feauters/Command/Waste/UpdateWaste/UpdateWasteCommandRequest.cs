namespace ecobony.application.Feauters.Command;

public class UpdateWasteCommandRequest:IRequest<UpdateWasteCommandResponse>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public decimal Weight { get; set; }
    public string Station { get; set; }
    public Guid CategoryId { get; set; }
}