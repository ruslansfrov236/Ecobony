namespace ecobony.persistence.Repository
{
    public class BalanceReadRepository(AppDbContext context):ReadRepository<Balance>(context), IBalanceReadRepository
    {
        
    }
}