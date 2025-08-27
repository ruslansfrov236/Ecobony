

namespace ecobony.persistence.Repository
{
    public class UserHistoryWriteRepository(AppDbContext _context):WriteRepository<UserHistory>(_context) , IUserHistoryWriteRepository
    {
    }
}
