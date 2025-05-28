namespace ecobony.persistence.Repository;

public class HeaderReadRepository(AppDbContext _context) :ReadRepository<Header>(_context), IHeaderReadRepository {}