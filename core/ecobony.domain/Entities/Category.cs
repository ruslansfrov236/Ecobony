namespace ecobony.domain.Entities;

public class Category:BaseEntity
{
 
    public int Count { get; set; }
    public string? Image { get; set; }
   
    public int Pointy { get; set; }
    [NotMapped]
    public IFormFile? FormFile { get; set; }
    
    public decimal PricePoint { get; set; } 

    public List<Waste>? Wastes { get; set; }
   
    public List<CategoryTranslation>? CategoryTranslations { get; set; }
}