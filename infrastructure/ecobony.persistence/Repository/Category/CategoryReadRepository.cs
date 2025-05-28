namespace ecobony.persistence.Repository;

public class CategoryReadRepository(AppDbContext _context) :ReadRepository<Category>(_context), ICategoryReadRepository {}