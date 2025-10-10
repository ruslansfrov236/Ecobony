

namespace ecobony.persistence.Repository
{
    public class LogOptionsWriteRepository(AppDbContext context) : WriteRepository<LogOptions>(context), ILogOptionWriteRepository
    {
    }
}
