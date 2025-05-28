namespace ecobony.domain.Entities;

public sealed class Header:BaseEntity
{
  
    public HeaderRole Role { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public IFormFile FomFile { get; set; }
    public List<HeaderTranslation> HeaderTranslations { get; set; }
}