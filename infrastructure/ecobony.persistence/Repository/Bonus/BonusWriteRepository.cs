namespace ecobony.persistence.Repository;

public class BonusWriteRepository(AppDbContext _context) : WriteRepository<Bonus>(_context),
    IBonusWriteRepository{}