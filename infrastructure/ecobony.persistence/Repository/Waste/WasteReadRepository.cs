using ecobony.application.Repository;

namespace ecobony.persistence.Repository;

public class WasteReadRepository(AppDbContext _context) : ReadRepository<Waste>(_context), IWasteReadRepository { }