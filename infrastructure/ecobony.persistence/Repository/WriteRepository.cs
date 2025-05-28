using ecobony.application.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ecobony.persistence.Repository;

public class WriteRepository<T>(AppDbContext _context): IWriteRepository<T> where T: BaseEntity
{
    private DbSet<T> Table => _context.Set<T>();
    public async Task<bool> AddAsync(T model)
    {
        EntityEntry<T> entityEntry = await Table.AddAsync(model);
        return entityEntry.State == EntityState.Added;
    }

    public async Task<bool> AddRangeAsync(List<T> model)
    {
        await Table.AddRangeAsync(model);
        return true;
    }

    public bool Update(T model)
    {
        EntityEntry<T> entityEntry =  Table.Update(model);
        return entityEntry.State == EntityState.Modified;
    }

    public bool UpdateRange(List<T> model)
    {
       Table.UpdateRange(model);
       return true;
    }

    public bool Delete(T model)
    {
        EntityEntry<T> entityEntry =  Table.Remove(model);
        return entityEntry.State == EntityState.Deleted;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        T?  data = await Table.FirstOrDefaultAsync(a => a.Id == Guid.Parse(id));

        return Delete(data);
    }

    public bool DeleteRange(List<T> model)
    {
        Table.RemoveRange(model);
        return true;
    }

    public async Task<int> SaveChangegesAsync(CancellationToken cancellationToken= default)
        => await _context.SaveChangesAsync(cancellationToken);
}