namespace ecobony.application.Feauters.Command.Header;

public class UpdateHeaderCommandRequest:IRequest<UpdateHeaderCommandResponse>
{
    public string Id { get; set; }
    public HeaderRole Role { get; set; }
    public string Image { get; set; }
    [NotMapped]
    
  
    public IFormFile FomFile { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid LanguageId { get; set; }
  
    public Guid HeaderId { get; set; }
   
}