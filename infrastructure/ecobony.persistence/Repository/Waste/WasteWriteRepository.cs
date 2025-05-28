using ecobony.application.Repository;

namespace ecobony.persistence.Repository;

public class WasteWriteRepository(AppDbContext _context) :WriteRepository<Waste>(_context), IWasteWriteRepository {}