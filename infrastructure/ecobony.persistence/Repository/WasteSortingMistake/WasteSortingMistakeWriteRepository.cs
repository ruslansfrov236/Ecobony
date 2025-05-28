namespace ecobony.persistence.Repository;

public class WasteSortingMistakeWriteRepository(AppDbContext _context)
    : WriteRepository<WasteSortingMistake>(_context), IWasteSortingMistakeWriteRepository { }