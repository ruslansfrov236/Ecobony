namespace ecobony.persistence.Repository;

public class SortingActionTranslationWriteRepository(AppDbContext _context)
    : WriteRepository<domain.Entities.SortingActionTranslation>(_context), ISortingActionTranslationWriteRepository { }