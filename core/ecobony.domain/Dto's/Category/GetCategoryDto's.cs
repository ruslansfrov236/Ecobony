namespace ecobony.domain.Dto_s;

public class GetCategoryDto_s
{
    public Guid CategoryId { get; set; }
    public Guid CategoryTranslationId { get; set; }
    public int Count { get; set; }

    public string Image { get; set; }
    public string Name { get; set; }
    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
    public string Description { get; set; }
    public  decimal Pointy { get; set; }
    public DateTime CreateAt {get; set; }
    public DateTime UpdateAt { get; set; }
    public bool isDeleted { get; set; }
  
    
}