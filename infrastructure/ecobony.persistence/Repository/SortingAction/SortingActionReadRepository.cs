namespace ecobony.persistence.Repository;

public class SortingActionReadRepository(AppDbContext _context)
    : ReadRepository<SortingAction>(_context), ISortingActionReadRepository { }