
namespace ecobony.domain.Entities;

public class BonusComunity : BaseEntity
{
    public Bonus? Bonus { get; set; }
    public Guid BonusId { get; set; }
    public Waste? Waste { get; set; }
    public decimal PricePoint { get; set; }
    public Guid WasteId { get; set; }
    public decimal Score { get; set; }
}