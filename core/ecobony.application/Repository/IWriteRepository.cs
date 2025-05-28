namespace ecobony.application.Repository;

public interface IWriteRepository<T>
{
    Task<bool> AddAsync(T model);
    Task<bool> AddRangeAsync(List<T> model);
    bool Update(T model);
    bool UpdateRange(List<T> model);
    bool Delete(T model);
    Task<bool> DeleteAsync(string id);
    bool DeleteRange(List<T> model);
    Task<int> SaveChangegesAsync(CancellationToken cancellationToken= default);
}