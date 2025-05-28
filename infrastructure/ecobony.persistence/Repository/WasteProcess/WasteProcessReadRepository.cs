using ecobony.application.Repository;

namespace ecobony.persistence.Repository;

public class WasteProcessReadRepository(AppDbContext _context) 
    :ReadRepository<WasteProcess>(_context), IWasteProcessReadRepository { }