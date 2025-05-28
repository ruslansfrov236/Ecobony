namespace ecobony.persistence.Repository;

public class WasteSortingMistakeTranslationReadRepository(
    AppDbContext _context) : ReadRepository<WasteSortingMistakeTranslation>(_context),
    IWasteSortingMistakeTranslationReadRepository { }