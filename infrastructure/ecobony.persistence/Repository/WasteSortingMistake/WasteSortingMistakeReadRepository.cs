namespace ecobony.persistence.Repository;

public class WasteSortingMistakeReadRepository(AppDbContext _context)
    : ReadRepository<WasteSortingMistake>(_context), IWasteSortingMistakeReadRepository { }