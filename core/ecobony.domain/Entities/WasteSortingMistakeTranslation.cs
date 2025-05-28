namespace ecobony.domain.Entities;

public class WasteSortingMistakeTranslation:BaseEntity
{
    public WasteSortingMistake WasteSortingMistake { get; set; }
    public Guid WasteSortingMistakeId { get; set; }
    public Language Language { get; set; }
    public Guid LanguageId { get; set; }
    public string Title { get; set; }
    public string Desctiption { get; set; }
}