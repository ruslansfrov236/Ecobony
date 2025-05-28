namespace ecobony.domain.Dto_s;

public class UpdateHeaderTranslationDto_s
{
    
    public string Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
    public Guid HeaderId { get; set; }
    public Header Header { get; set; }
}