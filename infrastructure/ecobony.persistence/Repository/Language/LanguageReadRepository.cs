namespace ecobony.persistence.Repository;

public class LanguageReadRepository(AppDbContext _context) :ReadRepository<Language>(_context), ILanguageReadRepository { }