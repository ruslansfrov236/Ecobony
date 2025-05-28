namespace ecobony.domain.Dto_s;

public class UpdateBonusDto_s
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public List<BonusComunity> BonusComunities { get; set; }
}