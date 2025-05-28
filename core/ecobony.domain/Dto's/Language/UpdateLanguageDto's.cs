namespace ecobony.domain.Dto_s;

public class UpdateLanguageDto_s
{
    public string Id { get; set; }
    public string IsoCode { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public  IFormFile FormFile { get; set; }
}