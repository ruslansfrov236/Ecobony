namespace ecobony.domain.Dto_s;

public class UpdateSortingActionTranslationDto_s
{
    public string Id { get; set; }
    public Guid SortingActionId   { get; set; }
    public SortingAction SortingAction { get; set; }
    public Guid LangugeId { get; set; }
    public Language? Language { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; } 
    public string Icon { get; set; } 
}