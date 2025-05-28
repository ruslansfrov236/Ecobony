namespace ecobony.persistence.Repository
{
    public class BalanceWriteRepository(AppDbContext context):WriteRepository<Balance>(context), IBalanceWriteRepository
    {
        
    }
}