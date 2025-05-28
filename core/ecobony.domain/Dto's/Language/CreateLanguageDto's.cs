namespace ecobony.domain.Dto_s;

public class CreateLanguageDto_s
{
    public string IsoCode { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public  IFormFile FormFile { get; set; }
}