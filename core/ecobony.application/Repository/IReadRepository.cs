


namespace ecobony.application.Repository;

public interface IReadRepository<T>
{
    IQueryable<T> GetAll(bool tracking = true);
    IQueryable<T> GetFilter(Expression<Func<T, bool>> method, bool tracking = true);
    Task<bool> AnyAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true);
    Task<T> GetByIdAsync(string id, bool tracking = true);
}