namespace ecobony.persistence.Repository;

public class HeaderTranslationWriteRepository(AppDbContext _context)
    : WriteRepository<HeaderTranslation>(_context), IHeaderTranslationWriteRepository { }