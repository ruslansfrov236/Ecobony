namespace ecobony.persistence.Repository;

public class BonusComunityReadRepository(AppDbContext _context)
    : ReadRepository<BonusComunity>(_context), IBonusComunityReadRepository { }