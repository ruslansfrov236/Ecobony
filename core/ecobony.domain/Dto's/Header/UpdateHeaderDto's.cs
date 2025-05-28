namespace ecobony.domain.Dto_s;

public class UpdateHeaderDto_s
{
    public string Id { get; set; }
    public HeaderRole Role { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public IFormFile FomFile { get; set; }
    public List<HeaderTranslation> HeaderTranslations { get; set; }
}