namespace ecobony.persistence.Repository;

public class LanguageWriteRepository(AppDbContext _context) :WriteRepository<Language>(_context), ILanguageWriteRepository { }