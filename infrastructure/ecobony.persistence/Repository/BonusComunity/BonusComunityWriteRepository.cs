namespace ecobony.persistence.Repository;

public class BonusComunityWriteRepository(AppDbContext _context)
    : WriteRepository<BonusComunity>(_context), IBonusComunityWriteRepository {}