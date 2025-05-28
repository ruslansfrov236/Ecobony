namespace ecobony.domain.Dto_s;

public class CreateBonusCommunityDto_s
{
    public Bonus Bonus { get; set; }
    public Guid BonusId { get; set; }
    public Waste Waste { get; set; }
    public Guid WasteId { get; set; }
    public decimal Score { get; set; }
}