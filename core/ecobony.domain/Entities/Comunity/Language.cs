namespace ecobony.domain.Entities.Comunity;

public class Language:BaseEntity
{
    public string IsoCode { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Key { get; set; }
    [NotMapped]
    public  IFormFile FormFile { get; set; }
}