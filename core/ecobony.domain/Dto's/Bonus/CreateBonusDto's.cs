namespace ecobony.domain.Dto_s;

public class CreateBonusDto_s
{
    public string UserId { get; set; }
    public AppUser User { get; set; }
    public List<BonusComunity> BonusComunities { get; set; }
}