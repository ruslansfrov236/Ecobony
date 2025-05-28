namespace ecobony.persistence.Repository;

public class HeaderTranslationReadRepository(AppDbContext _context)
    : ReadRepository<HeaderTranslation>(_context), IHeaderTranslationReadRepository { }