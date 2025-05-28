namespace ecobony.application.Feauters.Command;

public class CreateWasteCommandRequest:IRequest<CreateWasteCommandResponse>
{
    public string Title { get; set; }
    public decimal Weight { get; set; }
    public string Station { get; set; }
    public Guid CategoryId { get; set; }
}