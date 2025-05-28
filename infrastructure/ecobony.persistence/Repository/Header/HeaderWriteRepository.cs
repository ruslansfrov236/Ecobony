namespace ecobony.persistence.Repository;

public class HeaderWriteRepository(AppDbContext _context) :WriteRepository<Header>(_context), IHeaderWriteRepository
{
    
}