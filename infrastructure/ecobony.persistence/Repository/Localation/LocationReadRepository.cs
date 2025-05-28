namespace ecobony.persistence.Repository;

public class LocationReadRepository(AppDbContext _context) :ReadRepository<Location>(_context), ILocationReadRepository { }