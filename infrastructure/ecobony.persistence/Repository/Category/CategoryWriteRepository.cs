namespace ecobony.persistence.Repository;

public class CategoryWriteRepository(AppDbContext _context) :WriteRepository<Category>(_context), ICategoryWriteRepository { }