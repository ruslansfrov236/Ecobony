namespace ecobony.application.Feauters.Command.Language;

public class CreateLanguageCommandRequest:IRequest<CreateLanguageCommandResponse>
{
    public string IsoCode { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public  IFormFile FormFile { get; set; }
}