namespace ecobony.domain.Dto_s;

public class UpdateSortingActionDto_s
{
    public string Id { get; set; }
    public string NavigationUrl { get; set; }
    public List<SortingActionTranslation> SortingActionTranslations { get; set; }
}