namespace ecobony.domain.Entities;

public class HeaderTranslation:BaseEntity
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
    public Guid HeaderId { get; set; }
    public Header Header { get; set; }
    
}