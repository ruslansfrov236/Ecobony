namespace ecobony.domain.Entities;

public class CategoryTranslation:BaseEntity
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public Category Category { get; set; }
    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
    public string Description { get; set; }
}