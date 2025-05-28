using ecobony.application.Repository;

namespace ecobony.persistence.Repository;

public class WasteProcessWriteRepository(AppDbContext _context)
    : WriteRepository<WasteProcess>(_context), IWasteProcessWriteRepository { }