namespace ecobony.persistence.Repository;

public class SortingActionTranslationReadRepository(AppDbContext _context)
    : ReadRepository<SortingActionTranslation>(_context), ISortingActionTranslationReadRepository { }