namespace ecobony.application.Feauters.Command;

public class UpdateCategoryCommandRequest:IRequest<UpdateCategoryCommandResponse>
{
    public string Id { get; set; }
    public string Image { get; set; }
   
    public int Pointy { get; set; }
    [NotMapped]
    public IFormFile FormFile { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public Guid LanguageId { get; set; }
    public string Description { get; set; }
}