namespace ecobony.domain.Entities.Comunity;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreateAt {get; set; }
    public DateTime UpdateAt { get; set; }
    
   
    virtual public bool isDeleted { get; set; }
}