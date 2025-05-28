namespace ecobony.persistence.Repository;

public class SortingActionWriteRepository(AppDbContext _context) :WriteRepository<SortingAction>(_context), ISortingActionWriteRepository { }