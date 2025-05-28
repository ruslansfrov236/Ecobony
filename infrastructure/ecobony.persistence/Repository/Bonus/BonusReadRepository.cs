namespace ecobony.persistence.Repository;

public class BonusReadRepository(AppDbContext _context) :ReadRepository<Bonus>(_context), 
    IBonusReadRepository { }