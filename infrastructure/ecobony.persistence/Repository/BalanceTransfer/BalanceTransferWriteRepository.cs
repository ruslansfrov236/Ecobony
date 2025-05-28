namespace ecobony.persistence.Repository
{
    public class BalanceTransferWriteRepository(AppDbContext _context):WriteRepository<BalanceTransfer>(_context), IBalanceTransferWriteRepository
    {
        
    }
}