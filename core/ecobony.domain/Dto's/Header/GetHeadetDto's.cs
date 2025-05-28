namespace ecobony.domain.Dto_s;

public class GetHeadetDto
{
    public string Id { get; set; }
    public HeaderRole Role { get; set; }
    public string Image { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid LanguageId { get; set; }
    public Language Language { get; set; }
    public Guid HeaderId { get; set; }
    public DateTime CreateAt {get; set; }
    public DateTime UpdateAt { get; set; }
    public bool isDeleted { get; set; }
}