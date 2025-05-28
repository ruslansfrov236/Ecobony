namespace ecobony.persistence.Repository;

public class CategoryTranslationReadRepository(AppDbContext _context)
    : ReadRepository<CategoryTranslation>(_context), ICategoryTranslationReadRepository { }