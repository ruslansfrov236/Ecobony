namespace ecobony.persistence.Repository
{
    public class BalanceTransferReadRepository(AppDbContext _context):ReadRepository<BalanceTransfer>(_context), IBalanceTransferReadRepository
    {
        
    }
}