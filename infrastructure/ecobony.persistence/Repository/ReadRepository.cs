using System.Linq.Expressions;
using ecobony.application.Repository;

namespace ecobony.persistence.Repository;

public class ReadRepository<T>(AppDbContext _context):IReadRepository<T> where T :BaseEntity
{
    private DbSet<T> Table => _context.Set<T>();
    public IQueryable<T> GetAll(bool tracking = true)
     => tracking ? Table.AsQueryable() : Table.AsNoTracking();

    

    public IQueryable<T> GetFilter(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();
        return query.Where(method);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        return await query.AnyAsync(method);
    }

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        return await query.FirstOrDefaultAsync(method);
    }

    public async Task<T> GetByIdAsync(string id, bool tracking=true)
    {
        var query = tracking ? Table.AsQueryable() : Table.AsNoTracking();

        return await query.FirstOrDefaultAsync(a=>a.Id==Guid.Parse(id));
    }
}