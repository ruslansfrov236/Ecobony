namespace ecobony.persistence.Repository;

public class LocationWriteRepository(AppDbContext _context)
    : WriteRepository<Location>(_context), ILocationWriteRepository { }