namespace ecobony.domain.Entities;

public sealed class SortingAction:BaseEntity
{
  public string? NavigationUrl { get; set; }
  public List<SortingActionTranslation>? SortingActionTranslations { get; set; }
}