namespace ecobony.domain.Dto_s;
public class GetWasteSortingMistakeAll
{
    public string Id { get; set; }
    public WasteSortingMistake WasteSortingMistake { get; set; }
    public Guid WasteSortingMistakeId { get; set; }
    public Language Language { get; set; }
    public Guid LanguageId { get; set; }
    public string Title { get; set; }
    public string Desctiption { get; set; }
}