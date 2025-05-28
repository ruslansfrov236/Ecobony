namespace ecobony.persistence.Repository;

public class CategoryTranslationWriteRepository(AppDbContext _context)
    : WriteRepository<CategoryTranslation>(_context), ICategoryTranslationWriteRepository { }