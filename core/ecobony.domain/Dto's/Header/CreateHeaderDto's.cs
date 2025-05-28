namespace ecobony.domain.Dto_s;

public class CreateHeaderDto
{
    public HeaderRole Role { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public IFormFile FomFile { get; set; }
    public List<HeaderTranslation> HeaderTranslations { get; set; }
}