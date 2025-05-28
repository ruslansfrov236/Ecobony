namespace ecobony.domain.Dto_s;

public class UpdateCategoryDto_s
{
    public string Id { get; set;  }
    public int Count { get; set; }
    public string Image { get; set; }
    public decimal PricePoint { get; set; }
    public int Pointy { get; set; }
    [NotMapped]
    public IFormFile FormFile { get; set; }
    public List<CategoryTranslation> CategoryTranslations { get; set; }
}