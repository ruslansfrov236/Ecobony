namespace ecobony.application.Feauters.Command;

public class CreateCategoryCommandRequest:IRequest<CreateCategoryCommandResponse>
{
    

    public string Image { get; set; }
   
    public int Pointy { get; set; }
    [NotMapped]
    public IFormFile FormFile { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public Guid LanguageId { get; set; }
    public string Description { get; set; }
}
